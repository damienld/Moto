using Moto.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMoto
{
    public partial class Form1 : Form
    {
        public Color color = new Color();
        public Form1()
        {
            InitializeComponent();
        }

        private void btnReadData_Click(object sender, EventArgs e)
        {
            TextBox _textbox = edtFP1;
            DataGridView _grd = dGrdFP1;
            switch ((sender as Button).Text.ToLower().Replace(" ",""))
            {
                case "readfp2":
                    _textbox = edtFP2;
                    _grd = dGrdFP2;
                    break;
                case "readfp3":
                    _textbox = edtFP3;
                    _grd = dGrdFP3;
                    break;
                case "readfp4":
                    _textbox = edtFP4;
                    _grd = dGrdFP4;
                    break;
                case "readq1":
                    _textbox = edtQ1;
                    _grd = dGrdQ1;
                    break;
                case "readq2":
                    _textbox = edtQ2;
                    _grd = dGrdQ2;
                    break;
                case "readwup":
                    _textbox = edtWUP;
                    _grd = dGrdWUP;
                    break;
                case "readrace":
                    _textbox = edtRace;
                    _grd = dGrdRace;
                    break;
                default:
                    break;
            }
            List<string> _data = removeUselessLines(_textbox.Lines.ToList());

            _textbox.Lines = _data.ToArray();
            if (_data.FindIndex(l => l.Contains("Run #")) > -1)
                ReadLinesRidersAndLapsWithRunsDetails(_grd, _data);
            else
                readLinesRidersAndLaps(_grd, _data);
            
        }

        private static void ReadLinesRidersAndLapsWithRunsDetails(DataGridView _grd, List<string> _data)
        {
            SortableBindingList<RiderSession> newListRidersSession = new SortableBindingList<RiderSession>();
            List<RiderSession> listRidersSession = Moto.Data.Session.ReadLinesRidersAndLapsWithRunsDetails(_data);
            foreach (var riderSession in listRidersSession)
            {
                newListRidersSession.Add(riderSession);
            }

            _grd.DataSource = newListRidersSession;
            foreach (DataGridViewColumn column in _grd.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            _grd.Columns[0].Visible = false;
            _grd.Columns[1].Visible = false;
            _grd.Columns[2].Visible = false;
            _grd.Columns[6].Visible = false;
            _grd.Columns[8].Visible = false;
            _grd.Columns[10].Visible = false;
            _grd.Columns[9].Visible = false;
            _grd.Columns[3].Width = 20;
            _grd.Columns[4].Width = 4;
            _grd.Columns[5].Width = 1;

        }
        private static void readLinesRidersAndLaps(DataGridView _grd, List<string> _data)
        {
            SortableBindingList<RiderSession> newListRidersSession = new SortableBindingList<RiderSession>();
            List<RiderSession> listRidersSession = Moto.Data.Session.ReadLinesRidersAndLaps(_data);
            foreach (var riderSession in listRidersSession)
            {
                newListRidersSession.Add(riderSession);
            }

            _grd.DataSource = newListRidersSession;
            foreach (DataGridViewColumn column in _grd.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            _grd.Columns[0].Visible = false;
            _grd.Columns[1].Visible = false;
            _grd.Columns[2].Visible = false;
            _grd.Columns[6].Visible = false;
            _grd.Columns[8].Visible = false;
            _grd.Columns[10].Visible = false;
            _grd.Columns[9].Visible = false;
            _grd.Columns[3].Width = 20;
            _grd.Columns[4].Width = 4;
            _grd.Columns[5].Width = 1;

        }

        private List<string> removeUselessLines(List<string> _data)
        {
            SortableBindingList<RiderSession> _listRiders = new SortableBindingList<RiderSession>();
            //remove all rows till "Chronological "
            int _index1 = _data.FindIndex(l => l.StartsWith("Chronological "));
            for (int i = 0; i <= _index1; i++)
            {
                _data.RemoveAt(0);
            }
            //remove all interpage between rows "* Page X of X" and "TISSOT"
            Regex _reg = new Regex(@"^.*Page [0-9] of [0-9]$");
            int _index2 = _data.FindIndex(l => _reg.IsMatch(l));
            while (_index2 > -1)
            {
                int _index3 = _data.FindIndex(l => l.Trim().ToUpper() == "TISSOT");
                for (int i = _index2; i <= _index3; i++)
                {
                    _data.RemoveAt(_index2);
                }
                _index2 = _data.FindIndex(l => _reg.IsMatch(l));
            }
            //remove
            int _index4 = _data.FindIndex(l => l.EndsWith(" Speed"));
            while (_index4 > -1)
            {
                _data.RemoveAt(_index4);
                _index4 = _data.FindIndex(l => l.EndsWith(" Speed"));
            }
            int _index5 = _data.FindIndex(l => (l.EndsWith(" Moto2") || l.EndsWith(" Moto3") || l.EndsWith(" MotoGP"))
            && !l.EndsWith("Yamaha MotoGP"));
            while (_index5 > -1)
            {
                _data.RemoveAt(_index5);
                _index5 = _data.FindIndex(l => (l.EndsWith(" Moto2") || l.EndsWith(" Moto3") || l.EndsWith(" MotoGP"))
            && !l.EndsWith("Yamaha MotoGP"));
            }

            return _data;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] _allLines = System.IO.File.ReadAllLines(openFileDialog1.FileName);

                //FP1
                string[] _lines = _allLines.ToList().Where(l => l.StartsWith("[FP1]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[FP1]", "");
                }
                edtFP1.Lines = _lines;
                if (_lines.ToList().FindIndex(l => l.Contains("Run #")) > -1)
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdFP1, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdFP1, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[FP2]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[FP2]", "");
                }
                edtFP2.Lines = _lines;
                if (_lines.ToList().FindIndex(l => l.Contains("Run #")) > -1)
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdFP2, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdFP2, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[FP3]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[FP3]", "");
                }
                edtFP3.Lines = _lines;
                if (_lines.ToList().FindIndex(l => l.Contains("Run #")) > -1)
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdFP3, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdFP3, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[FP4]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[FP4]", "");
                }
                edtFP4.Lines = _lines;
                if (_lines.ToList().FindIndex(l => l.Contains("Run #")) > -1)
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdFP4, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdFP4, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[Q1]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[Q1]", "");
                }
                edtQ1.Lines = _lines;
                if (_lines.Contains("Run # 1"))
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdQ1, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdQ1, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[Q2]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[Q2]", "");
                }
                edtQ2.Lines = _lines;
                if (_lines.Contains("Run # 1"))
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdQ2, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdQ2, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[WUP]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[WUP]", "");
                }
                edtWUP.Lines = _lines;
                if (_lines.Contains("Run # 1"))
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdWUP, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdWUP, _lines.ToList());

                _lines = _allLines.ToList().Where(l => l.StartsWith("[RACE]")).ToArray();
                for (int i = 0; i <= _lines.Length - 1; i++)
                {
                    _lines[i] = _lines[i].Replace("[RACE]", "");
                }
                edtRace.Lines = _lines;
                if (_lines.Contains("Run #"))//NEW 22 07 20
                    ReadLinesRidersAndLapsWithRunsDetails(dGrdRace, _lines.ToList());
                else
                    readLinesRidersAndLaps(dGrdRace, _lines.ToList());
                foreach (DataGridView _grd in ListDataGridView)
                {
                    foreach (DataGridViewColumn column in _grd.Columns)
                    {

                        column.SortMode = DataGridViewColumnSortMode.Automatic;
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                List<string> _text = new List<string>();
                if (edtFP1.Text.Trim() != "")
                {
                    string[] _lines = edtFP1.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[FP1]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());

                }
                if (edtFP2.Text.Trim() != "")
                {
                    string[] _lines = edtFP2.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[FP2]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtFP3.Text.Trim() != "")
                {
                    string[] _lines = edtFP3.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[FP3]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtFP4.Text.Trim() != "")
                {
                    string[] _lines = edtFP4.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[FP4]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtQ1.Text.Trim() != "")
                {
                    string[] _lines = edtQ1.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[Q1]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtQ2.Text.Trim() != "")
                {
                    string[] _lines = edtQ2.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[Q2]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtRace.Text.Trim() != "")
                {
                    string[] _lines = edtRace.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[RACE]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                if (edtWUP.Text.Trim() != "")
                {
                    string[] _lines = edtWUP.Lines;
                    for (int i = 0; i <= _lines.Length - 1; i++)
                    {
                        _lines[i] = "[WUP]" + _lines[i];
                    }
                    _text.AddRange(_lines.ToList());
                }
                System.IO.File.WriteAllLines(saveFileDialog1.FileName, _text.ToArray());
            }
        }

        public List<DataGridView> ListDataGridView = new List<DataGridView>();
        public void initListDataGridView()
        {
            ListDataGridView.Clear();
            ListDataGridView.Add(dGrdFP1);
            ListDataGridView.Add(dGrdFP2);
            ListDataGridView.Add(dGrdFP3);
            ListDataGridView.Add(dGrdFP4);
            ListDataGridView.Add(dGrdQ1);
            ListDataGridView.Add(dGrdQ2);
            ListDataGridView.Add(dGrdWUP);
            ListDataGridView.Add(dGrdRace);
        }
        private void dGrdFP2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            initListDataGridView();
            if (e.ColumnIndex == 1 && e.RowIndex > -1)
            {
                DataGridView dg = sender as DataGridView;
                string _nameRider = dg[e.ColumnIndex, e.RowIndex].Value.ToString().Trim();
                //dg[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Blue;
                foreach (DataGridView _grd in ListDataGridView)
                {
                    for (int i = 1; i <= _grd.RowCount-1; i++)
                    {
                        string _cellValue = _grd[1, i].Value.ToString().Trim();
                        if (_cellValue == _nameRider)
                        {
                            _grd[0, i].Style.BackColor = Color.Red;
                            _grd.FirstDisplayedScrollingRowIndex = Math.Max(0, i - 8);
                        }
                        else
                            _grd[0, i].Style.BackColor = Color.White;
                    }
                }
            }
        }
    }
}
