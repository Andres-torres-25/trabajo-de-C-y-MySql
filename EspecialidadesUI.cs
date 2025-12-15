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
    public partial class EspecialidadesUI : Form
    {
        private EspecialidadDAO dao = new EspecialidadDAO();

        public EspecialidadesUI()
        {
            InitializeComponent();
        }

        private void EspecialidadesUILoad(object sender, EventArgs e)
        {
            FillGrid();
        }

        private void FillGrid()
        {
            try
            {
                dgEspecialidades.DataSource = dao.GetAll();
                dgEspecialidades.Refresh();
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void Clear()
        {
            txtld.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtld.Focus();
        }

        private bool IsValid(bool isRequiredID = false)
        {
            if (isRequiredID)
            {
                if (txtld.Text.Trim().Length == 0) return false;
            }
            if (txtNombre.Text.Trim().Length == 0) return false;
            if (txtDescripcion.Text.Trim().Length == 0) return false;
            return true;
        }

        private Especialidad? Capture(bool isRequiredID = false)
        {
            if (!IsValid(isRequiredID)) return null;

            Especialidad ob = new Especialidad();
            if (isRequiredID)
            {
                ob.Id = Convert.ToInt32(txtld.Text); 
            }
            ob.Nombre = txtNombre.Text.Trim().ToUpper();
            ob.Descripcion = txtDescripcion.Text.Trim().ToUpper();
            return ob;
        }

        private void FillData(int id)
        {
            try
            {
                var x = dao.GetById(id);
                if (x != null)
                {
                    txtID.Text = x.Id.ToString();
                    txtNombre.Text = x.Nombre;
                    txtDescripcion.Text = x.Descripcion;
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

        private void SaveData()
        {
            try
            {
                if (IsValid(false))
                {
                    if (WMHelper.Confirm("¿Desea agregar el registro?") == DialogResult.Yes)
                    {
                        var x = Capture(false);
                        if (x != null)
                        {
                            dao.Add(x);
                            FillGrid();
                            Clear();
                        }
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
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
                if (IsValid(true))
                {
                    if (WMHelper.Confirm("¿Desea actualizar el registro?") == DialogResult.Yes)
                    {
                        var x = Capture(true);
                        if (x != null)
                        {
                            dao.Update(x);
                            FillGrid();
                            Clear();
                        }
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
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
                if (IsValid(true))
                {
                    if (WMHelper.Confirm("¿Desea eliminar el registro?") == DialogResult.Yes)
                    {
                        var x = Capture(true);
                        if (x != null)
                        {
                            dao.Delete(x.Id);
                            FillGrid();
                            Clear();
                        }
                    }
                }
                else
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            Clear();
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

        private void dgEspecialidades_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int f = e.RowIndex;
            if (f >= 0)
            {
                FillData(Convert.ToInt32(dgEspecialidades.Rows[f].Cells[0].Value));
            }
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnCerrar = new Button();
            lblTittle = new Label();
            txtID = new Label();
            txtld = new TextBox();
            label2 = new Label();
            txtNombre = new TextBox();
            label1 = new Label();
            txtDescripcion = new TextBox();
            picIcon = new PictureBox();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            label3 = new Label();
            txtBuscar = new TextBox();
            btnBuscar = new Button();
            dgEspecialidades = new DataGridView();
            btnBuscarIcon = new Button();
            btnEliminarIcon = new Button();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgEspecialidades).BeginInit();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btnCerrar);
            pnlHeader.Controls.Add(lblTittle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(785, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnCerrar
            // 
            btnCerrar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCerrar.BackColor = Color.FromArgb(42, 115, 204);
            btnCerrar.FlatAppearance.BorderSize = 0;
            btnCerrar.FlatStyle = FlatStyle.Flat;
            btnCerrar.Location = new Point(742, 5);
            btnCerrar.Name = "btnCerrar";
            btnCerrar.Size = new Size(40, 40);
            btnCerrar.TabIndex = 1;
            btnCerrar.Text = "X";
            btnCerrar.UseVisualStyleBackColor = false;
            // 
            // lblTittle
            // 
            lblTittle.AutoSize = true;
            lblTittle.Font = new Font("Segoe UI", 14F);
            lblTittle.ForeColor = Color.White;
            lblTittle.Location = new Point(10, 10);
            lblTittle.Name = "lblTittle";
            lblTittle.Size = new Size(336, 38);
            lblTittle.TabIndex = 0;
            lblTittle.Text = "Gestión de Especialidades";
            // 
            // txtID
            // 
            txtID.AutoSize = true;
            txtID.Location = new Point(20, 70);
            txtID.Name = "txtID";
            txtID.Size = new Size(34, 25);
            txtID.TabIndex = 1;
            txtID.Text = "ID:";
            // 
            // txtld
            // 
            txtld.Location = new Point(110, 66);
            txtld.Name = "txtld";
            txtld.Size = new Size(120, 31);
            txtld.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 110);
            label2.Name = "label2";
            label2.Size = new Size(82, 25);
            label2.TabIndex = 3;
            label2.Text = "Nombre:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(110, 106);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(110, 31);
            txtNombre.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 150);
            label1.Name = "label1";
            label1.Size = new Size(108, 25);
            label1.TabIndex = 5;
            label1.Text = "Descripción:";
            // 
            // txtDescripcion
            // 
            txtDescripcion.Location = new Point(124, 152);
            txtDescripcion.Multiline = true;
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(106, 23);
            txtDescripcion.TabIndex = 6;
            // 
            // picIcon
            // 
            picIcon.BorderStyle = BorderStyle.FixedSingle;
            picIcon.Location = new Point(510, 56);
            picIcon.Name = "picIcon";
            picIcon.Size = new Size(160, 160);
            picIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picIcon.TabIndex = 7;
            picIcon.TabStop = false;
            // 
            // btnNuevo
            // 
            btnNuevo.FlatAppearance.BorderSize = 0;
            btnNuevo.FlatStyle = FlatStyle.System;
            btnNuevo.Location = new Point(452, 298);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(97, 30);
            btnNuevo.TabIndex = 34;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(555, 298);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(97, 30);
            btnGuardar.TabIndex = 35;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(658, 298);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(97, 30);
            btnActualizar.TabIndex = 36;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = true;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(555, 334);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(97, 30);
            btnEliminar.TabIndex = 37;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 322);
            label3.Name = "label3";
            label3.Size = new Size(67, 25);
            label3.TabIndex = 39;
            label3.Text = "Buscar:";
            // 
            // txtBuscar
            // 
            txtBuscar.Location = new Point(63, 319);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(155, 31);
            txtBuscar.TabIndex = 40;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(224, 322);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(80, 30);
            btnBuscar.TabIndex = 41;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // dgEspecialidades
            // 
            dgEspecialidades.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dgEspecialidades.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgEspecialidades.Location = new Point(0, 370);
            dgEspecialidades.Name = "dgEspecialidades";
            dgEspecialidades.RowHeadersWidth = 62;
            dgEspecialidades.Size = new Size(785, 157);
            dgEspecialidades.TabIndex = 42;
            // 
            // btnBuscarIcon
            // 
            btnBuscarIcon.Location = new Point(484, 222);
            btnBuscarIcon.Name = "btnBuscarIcon";
            btnBuscarIcon.Size = new Size(112, 34);
            btnBuscarIcon.TabIndex = 43;
            btnBuscarIcon.Text = "buscar";
            btnBuscarIcon.UseVisualStyleBackColor = true;
            // 
            // btnEliminarIcon
            // 
            btnEliminarIcon.Location = new Point(613, 222);
            btnEliminarIcon.Name = "btnEliminarIcon";
            btnEliminarIcon.Size = new Size(112, 34);
            btnEliminarIcon.TabIndex = 44;
            btnEliminarIcon.Text = "Eliminar";
            btnEliminarIcon.UseVisualStyleBackColor = true;
            // 
            // EspecialidadesUI
            // 
            ClientSize = new Size(785, 524);
            Controls.Add(btnEliminarIcon);
            Controls.Add(btnBuscarIcon);
            Controls.Add(dgEspecialidades);
            Controls.Add(btnBuscar);
            Controls.Add(txtBuscar);
            Controls.Add(label3);
            Controls.Add(btnEliminar);
            Controls.Add(btnActualizar);
            Controls.Add(btnGuardar);
            Controls.Add(btnNuevo);
            Controls.Add(picIcon);
            Controls.Add(txtDescripcion);
            Controls.Add(label1);
            Controls.Add(txtNombre);
            Controls.Add(label2);
            Controls.Add(txtld);
            Controls.Add(txtID);
            Controls.Add(pnlHeader);
            Name = "EspecialidadesUI";
            Load += EspecialidadesUI_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgEspecialidades).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        private void lblCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void EspecialidadesUI_Load(object sender, EventArgs e)
        {

        }

        private Panel pnlHeader;
        private Button btnCerrar;
        private Label lblTittle;
        private Label txtID;
        private TextBox txtld;
        private Label label2;
        private TextBox txtNombre;
        private Label label1;
        private TextBox txtDescripcion;
        private PictureBox picIcon;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnEliminar;
        private Label label3;
        private TextBox txtBuscar;
        private Button btnBuscar;
        private Button btnBuscarIcon;
        private Button btnEliminarIcon;
        private DataGridView dgEspecialidades;
    }
}
