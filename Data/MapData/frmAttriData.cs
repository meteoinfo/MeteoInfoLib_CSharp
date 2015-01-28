using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Layer;

namespace MeteoInfoC.Data.MapData
{
    public partial class frmAttriData : Form
    {
        //public int m_LayerHandle;
        private VectorLayer _layer;
        private bool m_CanEdit;
        private bool m_ContentChanged;

        public frmAttriData(VectorLayer layer)
        {
            InitializeComponent();

            _layer = layer;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void frmAttriData_Load(object sender, EventArgs e)
        {
            LoadDataTable();

            TSMI_StartEdit.Enabled = true;
            TSMI_StopEdit.Enabled = false;
            TSMI_AddField.Enabled = false;
            TSMI_RemoveField.Enabled = false;
            TSMI_RenameField.Enabled = false;
            m_CanEdit = false;
            m_ContentChanged = false;
        }

        private void LoadDataTable()
        {
            dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;
            BindingSource bindingSource = new BindingSource();
            //m_Layer = (VectorLayer)frmMain.CurrentWin.MapDocument.ActiveMapFrame.MapView.GetLayerFromHandle(aLayerHnd);
            DataTable aDataTable = _layer.AttributeTable.Table;
            bindingSource.DataSource = aDataTable;
            dataGridView1.DataSource = bindingSource;            
            dataGridView1.ReadOnly = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;            
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].ReadOnly = false;
            }
            dataGridView1.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            this.Text = "Attribute Data - " + _layer.LayerName;            
        }

        private void TSMI_StartEdit_Click(object sender, EventArgs e)
        {
            TSMI_StartEdit.Enabled = false;
            TSMI_StopEdit.Enabled = true;
            TSMI_AddField.Enabled = true;
            if (dataGridView1.SelectedColumns.Count > 0)
            {
                TSMI_RemoveField.Enabled = true;
                TSMI_RenameField.Enabled = true;
            }

            //dataGridView1.BeginEdit(False)
            m_CanEdit = true;
        }

        private void TSMI_StopEdit_Click(object sender, EventArgs e)
        {
            TSMI_StartEdit.Enabled = true;
            TSMI_StopEdit.Enabled = false;
            TSMI_AddField.Enabled = false;
            TSMI_RemoveField.Enabled = false;
            TSMI_RenameField.Enabled = false;

            m_CanEdit = false;            
            if (MessageBox.Show("If save the edition?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SaveDataTable();
                m_ContentChanged = false;
            }
        }

        private void SaveDataTable()
        {
            int i, j;
            for (j = 0; j < _layer.NumFields; j++)
            {
                for (i = 0; i < _layer.ShapeNum; i++)
                    _layer.EditCellValue(j, i, dataGridView1[j, i].Value);

            }
            _layer.AttributeTable.Save();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (m_CanEdit)
                dataGridView1.BeginEdit(true);

            TSMI_RemoveField.Enabled = false;
            TSMI_RenameField.Enabled = false;
        }

        private void TSMI_AddField_Click(object sender, EventArgs e)
        {
            frmAddField aFrmAF = new frmAddField();
            if (aFrmAF.ShowDialog() == DialogResult.OK)
            {
                string fieldName = aFrmAF.TB_Name.Text;
                if (fieldName == string.Empty)
                {
                    MessageBox.Show("Field name is empty!", "Error");
                    return;
                }
                List<string> FNList = GetFieldNameLsit();                
                if (FNList.Contains(fieldName))
                {
                    MessageBox.Show("Field name has exist in the data table!", "Error");
                    return;
                }

                Type aType;
                switch (aFrmAF.CB_Type.Text)
                {
                    case "String":
                        aType = typeof(string);
                        break;
                    case "Integer":
                        aType = typeof(int);
                        break;
                    default:
                        aType = typeof(double);
                        break;
                }

                _layer.EditAddField(new DataColumn(fieldName, aType));

                LoadDataTable();
                m_ContentChanged = true;
            }
        }

        private List<string> GetFieldNameLsit()
        {
            List<string> FNList = new List<string>();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                FNList.Add(dataGridView1.Columns[i].Name);
            }

            return FNList;
        }

        private void TSMI_RemoveField_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedColumns.Count == 0)
            {
                MessageBox.Show("Please select one field firstly!", "Alarm");
                return;
            }

            int fieldIdx = dataGridView1.SelectedColumns[0].Index;
            string fieldName = dataGridView1.Columns[fieldIdx].Name;
            if (MessageBox.Show("Remove field: " + fieldName + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _layer.EditDeleteField(fieldName);

                LoadDataTable();
                m_ContentChanged = true;
            }
        }

        private void TSMI_RenameField_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedColumns.Count == 0)
            {
                MessageBox.Show("Please select one field firstly!", "Alarm");
                return;
            }

            int fieldIdx = dataGridView1.SelectedColumns[0].Index;
            string fieldName = dataGridView1.Columns[fieldIdx].Name;
            frmInputBox aInputBox = new frmInputBox("Please input new field name：", "Change field name", fieldName);
            if (aInputBox.ShowDialog() == DialogResult.OK)
            {
                string aNewName = aInputBox.Value;
                if (aNewName == string.Empty)
                {
                    MessageBox.Show("Field name is empty!", "Error");
                    return;
                }
                List<string> FNList = GetFieldNameLsit();
                if (FNList.Contains(aNewName))
                {
                    MessageBox.Show("Field name has exist in the data table!", "Error");
                    return;
                }

                _layer.EditRenameField(fieldName, aNewName);

                LoadDataTable();
            }
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (m_CanEdit)
            {
                TSMI_RenameField.Enabled = true;
                TSMI_RemoveField.Enabled = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            m_ContentChanged = true;
        }

        private void frmAttriData_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_ContentChanged)
            {
                DialogResult ifSave = MessageBox.Show("If save the data table?", "Confirm", MessageBoxButtons.YesNoCancel);
                if (ifSave == DialogResult.Yes)
                {
                    SaveDataTable();
                }
                else if (ifSave == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            TSMI_RemoveField.Enabled = false;
            TSMI_RenameField.Enabled = false;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }        
    }
}
