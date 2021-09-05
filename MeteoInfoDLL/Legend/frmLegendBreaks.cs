using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MeteoInfoC.Shape;

namespace MeteoInfoC.Legend
{
    /// <summary>
    /// Create legend breaks form
    /// </summary>
    public partial class frmLegendBreaks : Form
    {
        #region Variables
        private object _parent;
        LegendScheme _legendScheme = null;
        double m_MinContourValue, m_MaxContourValue, m_Interval;
        bool _isUniqueValue = false;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">parent object</param>
        /// <param name="isUniqueValue">if is unique value legend scheme</param>
        public frmLegendBreaks(object parent, bool isUniqueValue)
        {
            InitializeComponent();

            _parent = parent;
            _isUniqueValue = isUniqueValue;
        }

        #endregion

        #region Methods
        private void frmLegendBreaks_Load(object sender, EventArgs e)
        {
            lab_Min.Text = "Min: " + _legendScheme.MinValue.ToString("e1");
            lab_Max.Text = "Max: " + _legendScheme.MaxValue.ToString("e1");
            if (_legendScheme.BreakNum > 2)
            {
                switch (_legendScheme.ShapeType)
                {
                    case ShapeTypes.Point:
                        PointBreak aPB = new PointBreak();
                        aPB = (PointBreak)_legendScheme.LegendBreaks[0];
                        m_MinContourValue = double.Parse(aPB.EndValue.ToString());
                        if (_legendScheme.HasNoData)
                        {
                            aPB = (PointBreak)_legendScheme.LegendBreaks[_legendScheme.BreakNum - 2];
                            m_MaxContourValue = double.Parse(aPB.StartValue.ToString());
                            m_Interval = (m_MaxContourValue - m_MinContourValue) / (_legendScheme.BreakNum - 3);
                        }
                        else
                        {
                            aPB = (PointBreak)_legendScheme.LegendBreaks[_legendScheme.BreakNum - 1];
                            m_MaxContourValue = double.Parse(aPB.StartValue.ToString());
                            m_Interval = (m_MaxContourValue - m_MinContourValue) / (_legendScheme.BreakNum - 2);
                        }                        
                        break;
                    case ShapeTypes.Polyline:
                    case ShapeTypes.PolylineZ:
                        PolyLineBreak aPLB = new PolyLineBreak();
                        aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[0];
                        m_MinContourValue = double.Parse(aPLB.EndValue.ToString());
                        aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[_legendScheme.BreakNum - 1];
                        m_MaxContourValue = double.Parse(aPLB.StartValue.ToString());
                        m_Interval = (m_MaxContourValue - m_MinContourValue) / (_legendScheme.BreakNum - 1);
                        break;
                    case ShapeTypes.Polygon:
                        PolygonBreak aPGB = new PolygonBreak();
                        aPGB = (PolygonBreak)_legendScheme.LegendBreaks[0];
                        m_MinContourValue = double.Parse(aPGB.EndValue.ToString());
                        aPGB = (PolygonBreak)_legendScheme.LegendBreaks[_legendScheme.BreakNum - 1];
                        m_MaxContourValue = double.Parse(aPGB.StartValue.ToString());
                        m_Interval = (m_MaxContourValue - m_MinContourValue) / (_legendScheme.BreakNum - 2);
                        break;
                }

                TB_SValue.Text = m_MinContourValue.ToString();
                TB_EValue.Text = m_MaxContourValue.ToString();
                TB_Interval.Text = m_Interval.ToString();
            }
            RB_RainbowColors.Checked = true;
        }

        /// <summary>
        /// Set legend scheme
        /// </summary>
        /// <param name="aLS"></param>
        public void SetLegendScheme(LegendScheme aLS)
        {
            _legendScheme = (LegendScheme)aLS.Clone();
        }

        private void RB_RainbowColors_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_RainbowColors.Checked)
            {
                Lab_StartColor.Enabled = false;
                Lab_EndColor.Enabled = false;
            }
        }

        private void RB_ShadeColors_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_ShadeColors.Checked)
            {
                Lab_StartColor.Enabled = true;
                Lab_EndColor.Enabled = true;
            }
        }

        private void Lab_StartColor_Click(object sender, EventArgs e)
        {
            ColorDialog aCDlg = new ColorDialog();
            if (aCDlg.ShowDialog() == DialogResult.OK)
            {
                Lab_StartColor.BackColor = aCDlg.Color;
            }
        }

        private void Lab_EndColor_Click(object sender, EventArgs e)
        {
            ColorDialog aCDlg = new ColorDialog();
            if (aCDlg.ShowDialog() == DialogResult.OK)
            {
                Lab_EndColor.BackColor = aCDlg.Color;
            }
        }

        private Color[] CreateColors(int colorNum)
        {
            Color[] colors;
            
            if (RB_RainbowColors.Checked)
            {
                colors = LegendManage.CreateRainBowColors(colorNum);
            }
            else
            {
                colors = LegendManage.CreateColors(Lab_StartColor.BackColor, Lab_EndColor.BackColor,
                    colorNum);
            }

            return colors;
        }

        private void B_NewColor_Click(object sender, EventArgs e)
        {
            int colorNum = _legendScheme.BreakNum;

            if (_legendScheme.ShapeType == ShapeTypes.Polyline)
            {
                colorNum += 1;
            }

            Color[] colors = CreateColors(colorNum);
            
            int i;
            switch (_legendScheme.ShapeType)
            {
                case ShapeTypes.Point:
                    PointBreak aPB = new PointBreak();
                    for (i = 0; i < _legendScheme.BreakNum; i++)
                    {
                        aPB = (PointBreak)_legendScheme.LegendBreaks[i];
                        aPB.Color = colors[i];
                        _legendScheme.LegendBreaks[i] = aPB;
                    }
                    break;
                case ShapeTypes.Polyline:
                case ShapeTypes.PolylineZ:
                    PolyLineBreak aPLB = new PolyLineBreak();
                    for (i = 0; i < _legendScheme.BreakNum; i++)
                    {
                        aPLB = (PolyLineBreak)_legendScheme.LegendBreaks[i];
                        aPLB.Color = colors[i];
                        _legendScheme.LegendBreaks[i] = aPLB;
                    }
                    break;
                case ShapeTypes.Polygon:
                    PolygonBreak aPGB = new PolygonBreak();
                    for (i = 0; i < _legendScheme.BreakNum; i++)
                    {
                        aPGB = (PolygonBreak)_legendScheme.LegendBreaks[i];
                        aPGB.Color = colors[i];
                        _legendScheme.LegendBreaks[i] = aPGB;
                    }
                    break;
            }

            if (_parent.GetType() == typeof(frmLegendSet))
            {
                ((frmLegendSet)_parent).SetLegendScheme(_legendScheme);
                ((frmLegendSet)_parent).legendView1.Update(_legendScheme);
            }
            else if (_parent.GetType() == typeof(LegendSchemeControl))
            {
                ((LegendSchemeControl)_parent).SetLegendScheme(_legendScheme);
                ((LegendSchemeControl)_parent).legendView1.Update(_legendScheme);
            }
        }

        private void B_NewLegend_Click(object sender, EventArgs e)
        {
            m_Interval = Convert.ToDouble(TB_Interval.Text);
            m_MinContourValue = Convert.ToDouble(TB_SValue.Text);
            m_MaxContourValue = Convert.ToDouble(TB_EValue.Text);
            //if (m_MinContourValue < _legendScheme.MinValue || 
            //    m_MaxContourValue > _legendScheme.MaxValue)
            //{
            //    MessageBox.Show("Please reset the data!", "Error");
            //    return;
            //}
            if ((int)((m_MaxContourValue - m_MinContourValue) / m_Interval) < 2)
            {
                MessageBox.Show("Please reset the data!", "Error");
                return;
            }

            double[] cValues;
            cValues = LegendManage.CreateContourValuesInterval(m_MinContourValue, m_MaxContourValue,
                m_Interval);

            Color[] colors = CreateColors(cValues.Length + 1);

            LegendScheme aLS;
            if (_isUniqueValue)
                aLS = LegendManage.CreateUniqValueLegendScheme(cValues, colors, _legendScheme.ShapeType,
                    _legendScheme.MinValue, _legendScheme.MaxValue, _legendScheme.HasNoData, _legendScheme.MissingValue);
            else
                aLS = LegendManage.CreateGraduatedLegendScheme(cValues, colors, _legendScheme.ShapeType,
                   _legendScheme.MinValue, _legendScheme.MaxValue, _legendScheme.HasNoData,
                   _legendScheme.MissingValue);
            aLS.FieldName = _legendScheme.FieldName;
            SetLegendScheme(aLS);

            if (_parent.GetType() == typeof(frmLegendSet))
            {
                ((frmLegendSet)_parent).SetLegendScheme(aLS);
                ((frmLegendSet)_parent).legendView1.Update(aLS);
            }
            else if (_parent.GetType() == typeof(LegendSchemeControl))
            {
                ((LegendSchemeControl)_parent).SetLegendScheme(aLS);
                ((LegendSchemeControl)_parent).legendView1.Update(aLS);
            }
        }

        #endregion
    }
}
