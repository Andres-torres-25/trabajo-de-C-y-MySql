using ClinicaApp._JP1._0.DAO;
using ClinicaApp._JP1._0.Helpers;
using ClinicaApp._JP1._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaApp._JP1._0.UI
{
    public partial class EPSUI : Form
    {
        private EPSDAO dao = new EPSDAO();

        public EPSUI()
        {
            InitializeComponent();
        }

        private void EPSUILoad(object sender, EventArgs e)
        {
            FillGrid();
            btnNuevo_Click(null, null);
        }

        private void FillGrid()
        {
            try
            {
                dgEps.DataSource = dao.GetAll();
                dgEps.Refresh();
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Clear()
        {
            txtID.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            btnNuevo_Click(null, null);
        }

        private bool IsValid(bool isRequiredID = false)
        {
            if (isRequiredID)
            {
                if (txtID.Text.Trim().Length == 0) return false;
            }
            if (txtNombre.Text.Trim().Length == 0) return false;
            if (txtDescripcion.Text.Trim().Length == 0) return false;
            return true;
        }

        private EPS? Capture(bool isRequiredID = false)
        {
            EPS ob = new EPS();
            if (isRequiredID)
            {
                ob.Id = Convert.ToInt32(txtID.Text);
            }
            ob.Nombre = txtNombre.Text.ToUpper();
            ob.Descripcion = txtDescripcion.Text.ToUpper();
            return ob;
        }

        private void SaveData()
        {
            try
            {
                if (!IsValid(false))
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                    return;
                }
                if (WMHelper.Confirm("¿Desea agregar el registro?") == DialogResult.Yes)
                {
                    dao.Add(Capture(false));
                    FillGrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void UpdateData()
        {
            try
            {
                if (!IsValid(true))
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                    return;
                }
                if (WMHelper.Confirm("¿Desea actualizar el registro?") == DialogResult.Yes)
                {
                    dao.Update(Capture(true));
                    FillGrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void DeleteData()
        {
            try
            {
                if (!IsValid(true))
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                    return;
                }
                if (WMHelper.Confirm("¿Desea eliminar el registro?") == DialogResult.Yes)
                {
                    dao.Delete(Convert.ToInt32(txtID.Text));
                    FillGrid();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void FillData(int id)
        {
            try
            {
                EPS? x = dao.GetById(id);
                if (x != null)
                {
                    txtID.Text = x.Id.ToString();
                    txtNombre.Text = x.Nombre.ToUpper();
                    txtDescripcion.Text = x.Descripcion.ToUpper();
                }
                else
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            txtID.ReadOnly = true;
            txtNombre.ReadOnly = true;
            txtDescripcion.ReadOnly = true;
            btnNuevo.Enabled = true;
            btnGuardar.Enabled = true;
            btnEliminar.Enabled = true;
            btnActualizar.Enabled = true;
            Clear();
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dgEPS_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int f = e.RowIndex;
            if (f >= 0)
            {
                FillData(Convert.ToInt32(dgEps[0, f].Value));
                txtID.ReadOnly = true;
                txtNombre.ReadOnly = true;
                txtDescripcion.ReadOnly = true;
                btnNuevo.Enabled = true;
                btnGuardar.Enabled = true;
                btnEliminar.Enabled = true;
                btnActualizar.Enabled = true;
            }
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btncerrar = new Button();
            lblTitle = new Label();
            pnlLeft = new Panel();
            btnNuevo = new Button();
            txtDescripcion = new TextBox();
            txtNombre = new TextBox();
            lblDescripcion = new Label();
            lblNombre = new Label();
            lblID = new Label();
            txtID = new TextBox();
            pnlRight = new Panel();
            pictureBox1 = new PictureBox();
            dgEps = new DataGridView();
            btnGuardar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            IDEPS = new DataGridViewTextBoxColumn();
            NombreEPS = new DataGridViewTextBoxColumn();
            Telefono = new DataGridViewTextBoxColumn();
            Estado = new DataGridViewTextBoxColumn();
            pnlHeader.SuspendLayout();
            pnlLeft.SuspendLayout();
            pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgEps).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btncerrar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1078, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btncerrar
            // 
            btncerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btncerrar.BackColor = Color.FromArgb(42, 115, 204);
            btncerrar.Cursor = Cursors.Hand;
            btncerrar.FlatAppearance.BorderSize = 0;
            btncerrar.FlatStyle = FlatStyle.Flat;
            btncerrar.ForeColor = Color.White;
            btncerrar.Location = new Point(1026, 3);
            btncerrar.Name = "btncerrar";
            btncerrar.Size = new Size(40, 40);
            btncerrar.TabIndex = 1;
            btncerrar.Text = "X";
            btncerrar.UseVisualStyleBackColor = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(10, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(203, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Gestión de EPS";
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(btnEliminar);
            pnlLeft.Controls.Add(btnActualizar);
            pnlLeft.Controls.Add(btnGuardar);
            pnlLeft.Controls.Add(btnNuevo);
            pnlLeft.Controls.Add(txtDescripcion);
            pnlLeft.Controls.Add(txtNombre);
            pnlLeft.Controls.Add(lblDescripcion);
            pnlLeft.Controls.Add(lblNombre);
            pnlLeft.Controls.Add(lblID);
            pnlLeft.Controls.Add(txtID);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Font = new Font("Segoe UI", 9F);
            pnlLeft.Location = new Point(0, 50);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(420, 594);
            pnlLeft.TabIndex = 1;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(42, 115, 204);
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(23, 228);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(110, 34);
            btnNuevo.TabIndex = 9;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            // 
            // txtDescripcion
            // 
            txtDescripcion.Font = new Font("Segoe UI", 9F);
            txtDescripcion.Location = new Point(120, 122);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(260, 80);
            txtDescripcion.TabIndex = 8;
            // 
            // txtNombre
            // 
            txtNombre.Font = new Font("Segoe UI", 9F);
            txtNombre.Location = new Point(89, 75);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(260, 31);
            txtNombre.TabIndex = 7;
            // 
            // lblDescripcion
            // 
            lblDescripcion.AutoSize = true;
            lblDescripcion.Font = new Font("Segoe UI", 9F);
            lblDescripcion.Location = new Point(10, 125);
            lblDescripcion.Name = "lblDescripcion";
            lblDescripcion.Size = new Size(104, 25);
            lblDescripcion.TabIndex = 3;
            lblDescripcion.Text = "Descripcion";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 9F);
            lblNombre.Location = new Point(10, 81);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(78, 25);
            lblNombre.TabIndex = 2;
            lblNombre.Text = "Nombre";
            // 
            // lblID
            // 
            lblID.AutoSize = true;
            lblID.Font = new Font("Segoe UI", 9F);
            lblID.Location = new Point(46, 29);
            lblID.Name = "lblID";
            lblID.Size = new Size(30, 25);
            lblID.TabIndex = 1;
            lblID.Text = "ID";
            // 
            // txtID
            // 
            txtID.Font = new Font("Segoe UI", 9F);
            txtID.Location = new Point(89, 22);
            txtID.Name = "txtID";
            txtID.Size = new Size(260, 31);
            txtID.TabIndex = 0;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(pictureBox1);
            pnlRight.Controls.Add(dgEps);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(420, 50);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(658, 594);
            pnlRight.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.EPS_Asmet_Salud_seguira_bajo_vigilancia_especial;
            pictureBox1.Location = new Point(260, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(150, 200);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // dgEps
            // 
            dgEps.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgEps.Columns.AddRange(new DataGridViewColumn[] { IDEPS, NombreEPS, Telefono, Estado });
            dgEps.Location = new Point(18, 219);
            dgEps.Margin = new Padding(10);
            dgEps.Name = "dgEps";
            dgEps.RowHeadersWidth = 62;
            dgEps.Size = new Size(628, 363);
            dgEps.TabIndex = 0;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(42, 115, 204);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(154, 228);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 34);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(42, 115, 204);
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(290, 228);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, 34);
            btnActualizar.TabIndex = 11;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(42, 115, 204);
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(154, 305);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(110, 34);
            btnEliminar.TabIndex = 12;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // IDEPS
            // 
            IDEPS.HeaderText = "ID";
            IDEPS.MinimumWidth = 8;
            IDEPS.Name = "IDEPS";
            IDEPS.ReadOnly = true;
            IDEPS.Width = 150;
            // 
            // NombreEPS
            // 
            NombreEPS.HeaderText = "Nombre EPS";
            NombreEPS.MinimumWidth = 8;
            NombreEPS.Name = "NombreEPS";
            NombreEPS.ReadOnly = true;
            NombreEPS.Width = 150;
            // 
            // Telefono
            // 
            Telefono.HeaderText = "Teléfono";
            Telefono.MinimumWidth = 8;
            Telefono.Name = "Telefono";
            Telefono.ReadOnly = true;
            Telefono.Width = 150;
            // 
            // Estado
            // 
            Estado.HeaderText = "Estado";
            Estado.MinimumWidth = 8;
            Estado.Name = "Estado";
            Estado.ReadOnly = true;
            Estado.Width = 150;
            // 
            // EPSUI
            // 
            ClientSize = new Size(1078, 644);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 12F);
            Name = "EPSUI";
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgEps).EndInit();
            ResumeLayout(false);

        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private Panel pnlHeader;
        private Label lblTitle;
        private Panel pnlLeft;
        private Label lblDescripcion;
        private Label lblNombre;
        private Label lblID;
        private TextBox txtID;
        private Panel pnlRight;
        private TextBox txtDescripcion;
        private TextBox txtNombre;
        private PictureBox pictureBox1;
        private DataGridView dgEps;
        private Button btnNuevo;
        private Button button2;
        private Button btnActualizar;
        private Button btnGuardar;
        private Button btnEliminar;
        private DataGridViewTextBoxColumn IDEPS;
        private DataGridViewTextBoxColumn NombreEPS;
        private DataGridViewTextBoxColumn Telefono;
        private DataGridViewTextBoxColumn Estado;
        private Button btncerrar;
    }
}
