using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Profiler.Helpers;
using System.Windows.Forms;
using System.IO;
using Aspose.Cells;
using System.Diagnostics;
using System.Globalization;

namespace Profiler
{
    public partial class Form1 : Form
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string fileLogName = "traceihrp";
        public string dirTrace = "";
        public string connectionstringSQL
        {
            get
            {
                var result = (txtConnection.Text.Trim() + ";Connect Timeout=6; Application Name=SQL Trace Log iHRP");
                return result.Replace(";;", ";");
            }
        }
        private static string ConfigFileLog
        {
            get
            {
                return "LogApp.xml";
            }
        }

        public Form1()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(ConfigFileLog));
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Load image help
                picHelp.ImageLocation = @"Icons\help.png";

                // Regristry aspose.cell
                Aspose.Cells.License ACL = new Aspose.Cells.License();
                ACL.SetLicense(LicenseAspose());

                // Load image radio button
                rbView.Image = Image.FromFile(@"Icons\grid.png");
                rbView.Text = "";
                rbView.Checked = true;
                rbExcel.Image = Image.FromFile(@"Icons\excel.png");
                rbExcel.Text = "";
                rbExcel.Checked = false;

                // Load connection string & log directory
                string[] lines = System.IO.File.ReadAllLines("Config.ini");
                var machinename = string.Empty;
                if (lines.Length >= 1)
                    txtDirLogSQL.Text = lines[0];
                if (lines.Length >= 2)
                    txtConnection.Text = clsCryptography.Decrypt(lines[1]);
                if (lines.Length >= 3)
                    machinename = lines[2];

                // Load info screen
                if (txtConnection.Text != "" && ValidateConnectionSQL())
                {
                    if (machinename.ToUpper() == System.Environment.MachineName)
                        btnRefresh_Click(null, null);
                }

                // Load report list
                lbReport.DataSource = GetListReportByLoginActive(1);

            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }


        // BIND DATAGRID
        private void GridBind()
        {
            // PROCESS FOR TRACE LOG
            var query = "select	a.id, case a.status when '1' then 'Running' when '0' then 'Stop' end as status, a.max_size, a.max_files, convert(varchar, a.start_time, 120) as start, convert(varchar, a.last_event_time, 120) as last, a.event_count as [count],a.path as [path_server] from sys.traces a where a.max_files is not null and is_default = 0;";
            var dtTrace = clsDataAccess.GetDataTableSql(query, connectionstringSQL);

            if (dtTrace != null && dtTrace.Rows.Count > 0)
                txtConnection.PasswordChar = '*';

            grvListTrace.Columns.Clear();
            grvListTrace.DataSource = dtTrace;

            if (dtTrace != null && dtTrace.Rows.Count > 0)
            {
                var imgDelete = new DataGridViewImageColumn();
                imgDelete.HeaderText = "delete";
                imgDelete.Name = "image";
                imgDelete.Image = Image.FromFile(@"Icons\btnDelete.jpg");
                grvListTrace.Columns.Insert(2, imgDelete);

                //format number
                if (grvListTrace.Columns["Count"] != null)
                    grvListTrace.Columns["Count"].DefaultCellStyle.Format = "N0";
            }
        }


        // EVENT GRID CELL
        private void grvListTrace_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2) // click column Delete
            {
                var traceId = grvListTrace.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                var status = "0";
                var sql1 = "EXEC sp_trace_setstatus @traceid = " + traceId + ", @status = " + status;

                status = "2";
                var sql2 = "EXEC sp_trace_setstatus @traceid = " + traceId + ", @status = " + status;

                var sql = sql1 + "; " + sql2;
                var confirmResult = MessageBox.Show("Do you want to delete this Trace ?", "Confirm Delete!!", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    var result = clsDataAccess.ExecuteSqlNoTran(sql, connectionstringSQL);
                    if (!result)
                        MessageBox.Show("Delete fail!");
                    else
                        btnRefresh_Click(null, null);
                }
            }
        }


        // EVENT TEXTBOX
        private void txtConnection_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateConnectionSQL())
                    return;
                else
                {
                    //Luu connection string vao line 2 & machine name vao line 3
                    string[] lines = System.IO.File.ReadAllLines("Config.ini");
                    lines[1] = clsCryptography.Encrypt(txtConnection.Text.Trim());
                    lines[2] = System.Environment.MachineName;
                    File.WriteAllLines("Config.ini", lines);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
        private void txtConnection_Enter(object sender, EventArgs e)
        {
            // Hidden connection string
            txtConnection.PasswordChar = '\0';
        }


        // EVENT BUTTON
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            { 
                // Check ket noi sql --> return if fail
                if (!ValidateConnectionSQL())
                    return;

                // Refresh Grid
                GridBind();

                // Resfresh List report template
                lbReport.DataSource = GetListReportByLoginActive(1);

                // Refresh Trace source
                var listLogFile = new List<string>();

                var flag = false;
                foreach (DataGridViewColumn column in grvListTrace.Columns)
                {
                    if (column.HeaderText.ToUpper() == "PATH_SERVER")
                    {
                        var pathfile = string.Empty;
                        for (int i = 0; i < grvListTrace.Rows.Count; i++)
                        {
                            pathfile = grvListTrace.Rows[i].Cells[column.Index].Value.ToString().Trim();
                            listLogFile.Add(pathfile);

                            if (pathfile.ToUpper().Contains("TRACEIHRP"))
                                flag = true;
                        }
                    }
                }

                // Update list log file
                var sql = @"SELECT DISTINCT REVERSE(SUBSTRING(REVERSE(path), CHARINDEX('\', REVERSE(path)), 256)) as [DirTrace] FROM sys.traces WHERE max_files is not null and is_default = 0";
                var dt = clsDataAccess.GetDataTableSql(sql, connectionstringSQL);
                var lstItem = new List<string>();

                Dictionary<string, string> source = new Dictionary<string, string>();
                var key = string.Empty;
                var value = string.Empty;

                object traceCurrent = null;
                if (cboTrace.Items.Count > 0)
                    traceCurrent = cboTrace.SelectedItem;

                if (dt != null)
                { 
                    foreach (DataRow dr in dt.Rows)
                    {
                        var query = "EXEC master.sys.xp_dirtree '" + dr["DirTrace"] + "',1,1";
                        var dtFile = clsDataAccess.GetDataTableSql(query, connectionstringSQL);

                        DataView dv = new DataView(dtFile);
                        dv.RowFilter = "file = 1";
                        dtFile = dv.ToTable();

                        if (dtFile.Rows.Count > 0) // Login Name have sysadmin role
                        { 
                            for (int i = 0; i < dtFile.Rows.Count; i++)
                            {
                                key = dr["DirTrace"] + "\\" + dtFile.Rows[i]["subdirectory"].ToString();    // Path full of file trace (VD: C:\\ihrptrace.trc)
                                value = dtFile.Rows[i]["subdirectory"].ToString();                          // Name of file trace   (VD: ihrptrace.trc)

                                if (value.ToUpper().Contains(".TRC") && value.ToUpper().Contains("TRACEIHRP"))
                                    source.Add(key, value);
                            }
                        }
                    }
                }

                if (source.Count == 0) // Login Name have alter trace role
                {
                    foreach (DataGridViewColumn column in grvListTrace.Columns)
                    {
                        if (column.HeaderText.ToUpper() == "PATH_SERVER")
                        {
                            for (int i = 0; i < grvListTrace.Rows.Count; i++)
                            {
                                key = grvListTrace.Rows[i].Cells[column.Index].Value.ToString().Trim();
                                value = Path.GetFileName(key);
                                source.Add(key, value);
                            }
                        }
                    }
                }

                if (source.Count > 0)
                {
                    cboTrace.DataSource = new BindingSource(source, null);
                    cboTrace.DisplayMember = "Value";
                    cboTrace.ValueMember = "Key";
                
                    if (traceCurrent != null && cboTrace.Items.Contains(traceCurrent))
                        cboTrace.SelectedItem = traceCurrent;
                }

                // Hidden or Show button Create Trace
                if (flag)
                    btnCreateTrace.Visible = false;
                else
                    btnCreateTrace.Visible = true;

                // Hidden or Show button Export
                if (cboTrace.Items.Count > 0)
                    btnExportReport.Visible = true;
                else
                    btnExportReport.Visible = false;
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
        private void btnCreateTrace_Click(object sender, EventArgs e)
        {
            var filename = fileLogName;

            try
            {
                if (txtDirLogSQL.Text == string.Empty)
                {
                    MessageBox.Show("Please input Directory Log of SQL SERVER");
                    return;
                }
                dirTrace = txtDirLogSQL.Text;
                var pathfile = dirTrace + "\\" + filename;
            
                //Declare sql
                var sql = new StringBuilder();
                sql.Append("declare @rc INT ");
                sql.Append("declare @TraceID INT ");
                sql.Append("declare @maxfilesize BIGINT = 300 ");
                sql.Append("declare @countfile INT = 30 ");

                sql.Append("declare @PathfileLog NVARCHAR(256) = N'" + pathfile + "' ");
                sql.Append("exec @rc = sp_trace_create @TraceID output, @options = 2, @tracefile = @PathfileLog, @maxfilesize = @maxfilesize, @stoptime = NULL, @filecount = @countfile ");
                sql.Append("declare @on BIT = 1 ");

                // Get Event: 12 T-SQL
                sql.Append("exec sp_trace_setevent @TraceID, 12, 1, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 9, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 6, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 16, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 17, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 18, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 31, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 48, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 12, 60, @on ");


                // Get Event: 81 SERVER: MEMORY CHANGE
                sql.Append("exec sp_trace_setevent @TraceID, 81, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 21, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 25, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 51, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 81, 64, @on ");

                // Get Event: DATABASE: AUTO GROW
                sql.Append("exec sp_trace_setevent @TraceID, 92, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 25, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 51, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 92, 64, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 25, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 51, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 94, 64, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 25, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 51, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 93, 64, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 25, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 51, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 95, 64, @on ");

                // Get Event: LOCK & LOCK CHAIN
                sql.Append("exec sp_trace_setevent @TraceID, 25, 1, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 9, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 32, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 57, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 60, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 25, 64, @on ");

                // Get Event: store procedure
                sql.Append("exec sp_trace_setevent @TraceID, 10, 1, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 9, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 2, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 10, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 3, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 6, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 8, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 11, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 12, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 13, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 14, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 15, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 16, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 17, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 18, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 26, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 31, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 35, @on ");
                sql.Append("exec sp_trace_setevent @TraceID, 10, 48, @on ");

                // SET FILTER
                sql.Append("exec sp_trace_setfilter @TraceID, 10, 0, 7, N'Microsoft SQL Server Management Studio - Transact-SQL IntelliSense' "); //ApplicationName
                sql.Append("exec sp_trace_setfilter @TraceID, 10, 0, 7, N'SQL Trace Log iHRP' ");//ApplicationName
                sql.Append("exec sp_trace_setfilter @TraceID, 1, 0, 7, N'exec sp_reset_connection' ");//TextData
                sql.Append("exec sp_trace_setstatus @TraceID, 1 ");

                var result = clsDataAccess.ExecuteSqlNoTran(sql.ToString(), connectionstringSQL);
                if (!result)
                {
                    var mgs = "Please review one of the information: \n 1) Account login has sysadmin or create profiler\n 2) Path Logs SQL Server must exits";
                    MessageBox.Show(mgs, "Don't Create Trace SQL", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    GridBind();
                    btnCreateTrace.Visible = false;

                    //write directory log
                    string[] lines = System.IO.File.ReadAllLines("Config.ini");
                    lines[0] = txtDirLogSQL.Text.Trim();
                    File.WriteAllLines("Config.ini", lines);
                }
            }

            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return;
            }
        }
        private void btnExportReport_Click(object sender, EventArgs e)
        {
            try
            { 
                btnRefresh.Visible = false;
                btnExportReport.Visible = false;

                // Report template
                if (tabReport.SelectedTab == tabReport.TabPages["tabPage1"])
                    ExportTemplate();

                // Report custom
                if (tabReport.SelectedTab == tabReport.TabPages["tabPage2"])
                {
                    // Don't action: update, delete, insert, truncate
                    var blackList = new List<string>() { "truncate", "delete", "update", "insert", "create", "alter", "drop", "exec"};
                    foreach(string key in blackList)
                    {
                        if (txtCustomReport.Text.ToLower().Contains(key))
                        {
                            MessageBox.Show("Allow only query with SELECT!");
                            return;
                        }
                    }
                    ExportCustom();
                }
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            finally
            {
                btnRefresh.Visible = true;
                btnExportReport.Visible = true;
            }
        }
        private void picHelp_Click(object sender, EventArgs e)
        {
            // View huong dan su dung
            System.Diagnostics.Process.Start(@"Helps\HelpTraceLog.pdf");
        }


        // EVENT SELECTED COMBOBOX
        private void cboTrace_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblInfoSource.Text = "...";
        }
        private void cboTrace_Leave(object sender, EventArgs e)
        {
            try
            { 
                btnRefresh.Visible = false;
                btnExportReport.Visible = false;

                var numberfile = upd_NumberFile.Value;
                var file = cboTrace.SelectedValue;
                var query = "SELECT left(convert(varchar, min(a.StartTime), 120), 16) as StartTime, left(convert(varchar, max(a.EndTime), 120), 16) as EndTime, count(a.StartTime) as [RowCount] FROM sys.fn_trace_gettable('" + file + "', " + numberfile.ToString() + ") a";
                var dtTime = clsDataAccess.GetDataRowSql(query, connectionstringSQL);

                if (dtTime != null)
                {
                    lblInfoSource.Text = "Row: " + string.Format(CultureInfo.InvariantCulture, "{0:N0}", dtTime["RowCount"]);
                    lblInfoSource.Text += "    Time: " + dtTime["StartTime"].ToString();
                    lblInfoSource.Text += " --> " + dtTime["EndTime"].ToString();
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }

            finally
            {
                btnRefresh.Visible = true;
                if (cboTrace.Items.Count > 0)
                    btnExportReport.Visible = true;
            }
        }

        // EVENT DOUBLE CLICK
        private void lbReport_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                // Get query of tempate report
                var pathLog = cboTrace.SelectedValue.ToString();
                var numberfile = upd_NumberFile.Value.ToString();
                var reportId = lbReport.SelectedIndex;

                var lineReport = GetListReportByLoginActive(2);
                var query = lineReport[reportId].Replace("@PathFile", pathLog);
                query = query.Replace("@NumberFile", numberfile);

                // Set query for report custom
                txtCustomReport.Text = query;
            }
            catch(Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }


        //EVENT FUNCTION FRAMEWORK
        private Stream LicenseAspose()
        {
            string xmlLicense = string.Empty;
            Stream license;

            xmlLicense = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
            xmlLicense = xmlLicense + "<License>";
            xmlLicense = xmlLicense + "<Data>";
            xmlLicense = xmlLicense + "<LicensedTo>FPT Information system</LicensedTo>";
            xmlLicense = xmlLicense + "<EmailTo>hoangnt16@fpt.com.vn</EmailTo>";
            xmlLicense = xmlLicense + "<LicenseType>Developer OEM</LicenseType>";
            xmlLicense = xmlLicense + "<LicenseNote>Limited to 1 developer.</LicenseNote>";
            xmlLicense = xmlLicense + "<OrderID>141002051606</OrderID>";
            xmlLicense = xmlLicense + "<UserID>275358</UserID>";
            xmlLicense = xmlLicense + "<OEM>This is a redistributable license</OEM>";
            xmlLicense = xmlLicense + "<Products>";
            xmlLicense = xmlLicense + "<Product>Aspose.Total Product Family</Product>";
            xmlLicense = xmlLicense + "</Products>";
            xmlLicense = xmlLicense + "<EditionType>Enterprise</EditionType>";
            xmlLicense = xmlLicense + "<SerialNumber>72d50801-1a9e-4899-905d-38307ee51a36</SerialNumber>";
            xmlLicense = xmlLicense + "<SubscriptionExpiry>20151015</SubscriptionExpiry>";
            xmlLicense = xmlLicense + "<LicenseVersion>3.0</LicenseVersion>";
            xmlLicense = xmlLicense + "<LicenseInstructions>http://www.aspose.com/corporate/purchase/license-instructions.aspx</LicenseInstructions>";
            xmlLicense = xmlLicense + "</Data>";
            xmlLicense = xmlLicense + "<Signature>ggrtgqzpRY7YE5HxnSYGg+B9m3i4x2jqVG2ywtqMZ9vEq1qQOLwOJ2q0v+kzxiOGEFUWWaF6KV4Bd14FcKRM5J/BA0HncoDpoHJGaSzyRNskFklKkZE80BiWwt30cZH88x9auvHpf+ppAdB6AY7+CFla4ciO1Jyt/2wQ5cKms1g=</Signature>";
            xmlLicense = xmlLicense + "</License>";
            license = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(xmlLicense));
            return license;
        }
        private bool ValidateConnectionSQL()
        {
            // Check connection
            if (!clsDataAccess.CheckConnectSQL(connectionstringSQL))
            {
                var mgs = "Connection to SQL Server Error or Server is not running. \nPlease review one of the information: \n 1) Server \n 2) User \n 3) password!";
                MessageBox.Show(mgs, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                return false;
            }

            return true;

            // 2016 Apr 07: remove
            //if (!clsDataAccess.CheckUserIsSysAdmin(connectionstringSQL))
            //{
            //    MessageBox.Show("Please choose user with sysadmin rule!");
            //    return false;
            //}
        }

        private List<string> GetListReportByLoginActive(int type) //type = 1: list report & type = 2: list query
        {
            string[] lineReport = System.IO.File.ReadAllLines("Query.ini");
            var row = "";
            var lst = new List<string>();

            if (clsDataAccess.CheckUserIsSysAdmin(connectionstringSQL))
            {
                foreach (string item in lineReport)
                {
                    if (type == 1)
                        row = item.Substring(item.IndexOf(@"/*") + 2, item.IndexOf(@"*/") - 2).Replace("--", "");
                    else if (type == 2)
                        row = item.Substring(item.IndexOf(@"*/") + 2, item.Length - item.IndexOf(@"*/") - 2);

                    lst.Add(row.Trim());
                }
            }

            else
            {
                foreach (string item in lineReport)
                {
                    if (type == 1)
                        row = item.Substring(item.IndexOf(@"/*") + 2, item.IndexOf(@"*/") - 2);
                    else if (type == 2)
                        row = item.Substring(item.IndexOf(@"*/") + 2, item.Length - item.IndexOf(@"*/") - 2);

                    if (!item.ToLower().Contains(@"/*--"))
                        lst.Add(row.Trim());
                }
            }
            return lst;
        }

        //Export gridview & excel file
        private void ExportTemplate()
        {
            try
            {
                var pathLog = cboTrace.SelectedValue.ToString();
                var numberfile = upd_NumberFile.Value.ToString();
                var reportId = lbReport.SelectedIndex;

                // create data of report
                var lineReport = GetListReportByLoginActive(2);
                var query = lineReport[reportId].Replace("@PathFile", pathLog);
                query = query.Replace("@NumberFile", numberfile);
                var dt = clsDataAccess.GetDataTableSql(query, connectionstringSQL);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data is not found!", "RESULT QUERY", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }

                // Export to grid
                if (rbView.Checked == true)
                {
                    DataTable dtReprort = new DataTable();
                    DataColumn AutoNumberColumn = new DataColumn();
                    AutoNumberColumn.ColumnName = "No.";
                    AutoNumberColumn.DataType = typeof(int);
                    AutoNumberColumn.AutoIncrement = true;
                    AutoNumberColumn.AutoIncrementSeed = 1;
                    AutoNumberColumn.AutoIncrementStep = 1;
                    dtReprort.Columns.Add(AutoNumberColumn);
                    dtReprort.Merge(dt);

                    // declare grid
                    DataGridView dgv = new DataGridView();
                    dgv.AllowUserToAddRows = false;
                    dgv.AllowUserToDeleteRows = false;
                    dgv.AllowUserToResizeColumns = true;
                    dgv.AllowUserToResizeRows = false;
                    dgv.Name = "dgv";
                    dgv.ReadOnly = true;
                    dgv.RowHeadersVisible = true;
                    dgv.Size = new System.Drawing.Size(800, 500);
                    dgv.DataSource = dtReprort;
                    dgv.Dock = DockStyle.Fill;


                    // declare form report
                    var myForm = new Form2();
                    myForm.Controls.Add(dgv);
                    myForm.Show();
                    myForm.MaximizeBox = true;
                    myForm.MinimizeBox = true;
                    

                    FormatReportGrid(dgv);
                }

                // Export to excel
                if (rbExcel.Checked == true)
                {
                    // Declare file excel
                    Workbook wb = new Workbook();
                    var ws = wb.Worksheets[0];
                    ws.Cells.ImportDataTable(dt, true, 0, 0, true);
                    ws = FormatReportExcel(ws);

                    // Save report excel
                    var name = lbReport.Text.Replace(" ", "_").Replace(".", "_") + ".xlsx";
                    var fileName = "Reports\\" + name;
                    if (clsProcessFile.CheckStatusFile(fileName) == 2)
                    {
                        MessageBox.Show("Vui lòng close file excel: \n" + name);
                        return;
                    }
                    wb.Save(fileName, SaveFormat.Xlsx);
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }
        private void ExportCustom()
        {
            try
            {
                var dt = clsDataAccess.GetDataTableSql(txtCustomReport.Text.Trim(), connectionstringSQL);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Data is not found!", "Result query", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    return;
                }

                // Export to grid
                if (rbView.Checked == true)
                {
                    DataTable dtReprort = new DataTable();
                    DataColumn AutoNumberColumn = new DataColumn();
                    AutoNumberColumn.ColumnName = "No.";
                    AutoNumberColumn.DataType = typeof(int);
                    AutoNumberColumn.AutoIncrement = true;
                    AutoNumberColumn.AutoIncrementSeed = 1;
                    AutoNumberColumn.AutoIncrementStep = 1;
                    dtReprort.Columns.Add(AutoNumberColumn);
                    dtReprort.Merge(dt);

                    // declare grid
                    DataGridView dgv = new DataGridView();
                    dgv.AllowUserToAddRows = false;
                    dgv.AllowUserToDeleteRows = false;
                    dgv.AllowUserToResizeColumns = true;
                    dgv.AllowUserToResizeRows = false;
                    dgv.Name = "dgv";
                    dgv.ReadOnly = true;
                    dgv.RowHeadersVisible = true;
                    dgv.Size = new System.Drawing.Size(800, 500);
                    dgv.DataSource = dtReprort;

                    // declare form report
                    var myForm = new Form2();
                    myForm.Controls.Add(dgv);
                    myForm.Show();

                    FormatReportGrid(dgv);
                }

                // Export to excel
                if (rbExcel.Checked == true)
                {
                    // Declare file excel
                    Workbook wb = new Workbook();
                    var ws = wb.Worksheets[0];
                    ws.Cells.ImportDataTable(dt, true, 0, 0, true);
                    ws = FormatReportExcel(ws);

                    // Save and open file excel
                    var name = "QueryCustom.xlsx";
                    var fileName = "Reports\\" + name;
                    if (clsProcessFile.CheckStatusFile(fileName) == 2)
                    {
                        MessageBox.Show("Vui lòng close file excel: \n" + name);
                        return;
                    }
                    wb.Save(fileName, SaveFormat.Xlsx);
                    Process.Start(fileName);
                }
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Exception: {0}", ex.Message);
                MessageBox.Show(ex.Message, "EXCEPTION", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

            //Format report with gridview & excel file
        private Worksheet FormatReportExcel(Worksheet ws)
        {
            // Format file
            var style = ws.Cells.Rows[0].Style;
            style.Font.IsBold = true;
            style.Font.Size = 8;
            style.Number = 3;


            var flag = new StyleFlag();
            flag.FontBold = true;
            flag.FontSize = true;
            flag.NumberFormat = false;

            // ----- Bold for header
            ws.Cells.Rows[0].ApplyStyle(style, flag);
            for (var i = 1; i <= ws.Cells.MaxDataRow; i++)
            {
                flag.FontBold = false;
                flag.FontSize = true;
                flag.NumberFormat = false;
                ws.Cells.Rows[i].ApplyStyle(style, flag);
            }

            // ----- list full format
            var listNumber = new List<string> { "Count", "Duration", "Read", "Write", "CPU", "hit" };
            var listdatetime = new List<string> { "Time" };
            var listdate = new List<string> { "Date", "Day" };


            flag.FontBold = false;
            flag.FontSize = true;
            flag.NumberFormat = true;

            // ----- Rule 1: format data type is number
            for (var i = 0; i <= ws.Cells.MaxColumn; i++)
            {
                foreach (string field in listNumber)
                {
                    if (ws.Cells[0, i].Value.ToString().ToUpper().Contains(field.ToUpper())) // format is number
                    {
                        ws.Cells.Columns[i].ApplyStyle(style, flag);
                    }
                }
            }

            // ----- Rule 2: format data typeis datetime
            for (var i = 0; i < ws.Cells.MaxColumn; i++)
            {
                style.Custom = "yyyy/mm/dd hh:mm:ss";
                foreach (string field in listdatetime)
                {
                    if (ws.Cells[0, i].Value.ToString().ToUpper().Contains(field.ToUpper())) // column had format is datetime
                    {
                        ws.Cells.Columns[i].ApplyStyle(style, flag);
                    }
                }
            }

            // ----- Rule 3: format data type is date
            for (var i = 0; i < ws.Cells.MaxColumn; i++)
            {
                style.Custom = "yyyy/mm/dd";
                foreach (string field in listdate)
                {
                    if (ws.Cells[0, i].Value.ToString().ToUpper().Contains(field.ToUpper())) // column had format is date
                    {
                        ws.Cells.Columns[i].ApplyStyle(style, flag);
                    }
                }
            }

            // ----- auto fit for column
            for (var i = 0; i < ws.Cells.MaxColumn; i++)
            {
                if (ws.Cells[0, i].Value.ToString().ToUpper() != "TEXTDATA")
                {
                    ws.AutoFitColumn(i);
                }
            }

            return ws;
        }
        private void FormatReportGrid(DataGridView dgv)
        {
            // ----- list full format
            var listNumber = new List<string> { "Count", "Duration", "Read", "Write", "CPU", "hit" };
            var listdatetime = new List<string> { "Time" };
            var listdate = new List<string> { "Date", "Day" };

            // ----- Rule 1: format data type is number
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                foreach (string field in listNumber)
                {
                    if (column.HeaderText.ToUpper().Contains(field.ToUpper()))
                    {
                        column.DefaultCellStyle.Format = "N0";
                    }
                }
            }

            // ----- Rule 2: format data type is datetime
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                foreach (string field in listdatetime)
                {
                    if (column.HeaderText.ToUpper().Contains(field.ToUpper()))
                    {
                        column.DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                    }
                }
            }

            // ----- Rule 3: format data type is date
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                foreach (string field in listdate)
                {
                    if (column.HeaderText.ToUpper().Contains(field.ToUpper()))
                    {
                        column.DefaultCellStyle.Format = "yyyy/MM/dd";
                    }
                }
            }
        }
    }
}