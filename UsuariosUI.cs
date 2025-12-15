using ClinicaApp._JP1._0;
using ClinicaApp._JP1._0.DAO;
using ClinicaApp._JP1._0.Helpers;
using ClinicaApp._JP1._0.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaClinicaMySQL.UI
{
    public partial class UsuariosUI : Form
    {
        private UsuarioDAO dao = new UsuarioDAO();

        public UsuariosUI()
        {
            InitializeComponent();
        }

        private void ClearData()
        {
            txtId.Clear();
            txtUsuario.Clear();
            txtContraseña.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtEmail.Clear();
            txtTelefono.Clear();
            if (cbxRol.Items.Count > 0) cbxRol.SelectedIndex = 0;
            if (cbxEstado.Items.Count > 0) cbxEstado.SelectedIndex = 0;
            txtId.ReadOnly = true;
        }

        private void FillData(int id)
        {
            Usuario? u = dao.GetById(id);
            if (u != null)
            {
                txtId.Text = u.Id.ToString();
                txtUsuario.Text = u.UserName;
                txtNombre.Text = u.Nombre;
                txtApellido.Text = u.Apellido;
                txtEmail.Text = u.Email;
                cbxRol.Text = u.Rol;
                dtpFechaCreacion.Value = u.FechaCreacion;
                if (u.Foto != null && u.Foto.Length > 0)
                {
                    picUsuario.Image = WMImage.BytesToImage(u.Foto);
                }
                txtId.ReadOnly = true;
                btnActualizar.Enabled = true;
                btnEliminar.Enabled = true;
            }
            else
            {
                ClearData();
            }
        }

        private void FillGrid()
        {
            try
            {
                dgUsuario.AutoGenerateColumns = false;
                dgUsuario.DataSource = dao.GetAll();
                dgUsuario.Refresh();
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private bool IsValid()
        {
            if (txtId.Text.Trim().Length == 0) return false;
            if (txtUsuario.Text.Trim().Length == 0) return false;
            if (txtContraseña.Text.Trim().Length == 0) return false;
            if (txtNombre.Text.Trim().Length == 0) return false;
            if (txtApellido.Text.Trim().Length == 0) return false;
            if (txtEmail.Text.Trim().Length == 0) return false;
            if (cbxRol.SelectedIndex == -1) return false;
            return true;
        }

        private Usuario CaptureData()
        {
            int id = 0;
            int.TryParse(txtId.Text, out id);

            Usuario ob = new Usuario
            {

                Id = id,
                UserName = txtUsuario.Text.Trim().ToUpper(),
                Password = WMCoder.Code(txtContraseña.Text),
                Nombre = txtNombre.Text.Trim().ToUpper(),
                Apellido = txtApellido.Text.Trim().ToUpper(),
                Email = txtEmail.Text.Trim().ToUpper(),
                Rol = cbxRol.Text,
                FechaCreacion = dtpFechaCreacion.Value,
                Foto = IsDefaultUserImage(picUsuario.Image) ? null : WMImage.ImageToBytes(picUsuario.Image)
            };
            return ob;
        }

        private bool IsDefaultUserImage(Image? image)
        {
            throw new NotImplementedException();
        }

        private void SaveData()
        {
            try
            {
                if (!IsValid())
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                    return;
                }
                if (WMHelper.Confirm("¿Desea agregar el registro?") == DialogResult.Yes)
                {
                    dao.Add(CaptureData());
                    FillGrid();
                    ClearData();
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
                if (!IsValid())
                {
                    WMHelper.ShowError("Faltan valores requeridos");
                    return;
                }
                if (WMHelper.Confirm("¿Desea actualizar el registro?") == DialogResult.Yes)
                {
                    dao.Update(CaptureData());
                    FillGrid();
                    ClearData();
                    txtId.ReadOnly = false;
                    btnActualizar.Enabled = false;
                    btnEliminar.Enabled = false;
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
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    WMHelper.ShowError("Seleccione un registro");
                    return;
                }
                if (Program.UsuarioLogeado != null && Convert.ToInt32(txtId.Text) == Program.UsuarioLogeado.Id)
                {
                    WMHelper.ShowError("No se puede eliminar el usuario actual");
                    return;
                }
                if (WMHelper.Confirm("¿Desea eliminar el registro?") == DialogResult.Yes)
                {
                    dao.Delete(Convert.ToInt32(txtId.Text));
                    FillGrid();
                    ClearData();
                }
            }
            catch (Exception ex)
            {
                WMHelper.ShowError(ex.Message);
            }
        }

        private void SearchImage()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Images (*.JPG;*.PNG)|*.JPG;*.PNG";
            if (op.ShowDialog() == DialogResult.OK)
            {
                picUsuario.ImageLocation = op.FileName;
            }
        }

        // EVENTOS DE BOTONES
        private void UsuariosUILoad(object sender, EventArgs e)
        {
            cbxRol.Items.AddRange(new string[] { "Administrador", "Usuario" });
            cbxEstado.Items.AddRange(new string[] { "Activo", "Inactivo" });
            if (cbxRol.Items.Count > 0) cbxRol.SelectedIndex = 0;
            if (cbxEstado.Items.Count > 0) cbxEstado.SelectedIndex = 0;
            ClearData();
            FillGrid();
        }

        private void btnNuevoClick(object sender, EventArgs e)
        {
            ClearData();
            txtId.ReadOnly = false;
        }

        private void btnGuardarClick(object sender, EventArgs e)
        {
            SaveData();
        }

        private void btnActualizarClick(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void btnEliminarClick(object sender, EventArgs e)
        {
            DeleteData();
        }

        private void btnLimpiarClick(object sender, EventArgs e)
        {
            ClearData();
        }

        private void picBuscarFotoClick(object sender, EventArgs e)
        {
            SearchImage();
        }



        private void picEliminarFoto_Click(object sender, EventArgs e)
        {
            // Restablece la imagen por defecto; si no existe el recurso user1, se pone null
            try
            {
                picUsuario.Image = Properties.Resources.user1;
            }
            catch
            {
                picUsuario.Image = null;
            }
        }

        private void dgUsuariosCellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int f = e.RowIndex;
            if (f >= 0)
            {
                FillData(Convert.ToInt32(dgUsuario.Rows[f].Cells[0].Value));
            }
        }

        private void lblCerrarClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnClose = new Button();
            lblTitle = new Label();
            splitMain = new SplitContainer();
            picUsuario = new PictureBox();
            dtpFechaCreacion = new DateTimePicker();
            lblFechaCreacion = new Label();
            label1 = new Label();
            txtNombre = new TextBox();
            flpButtons = new FlowLayoutPanel();
            btnNuevo = new Button();
            btnGuardar = new Button();
            btnActualizar = new Button();
            btnEliminar = new Button();
            cbxEstado = new ComboBox();
            cbxRol = new ComboBox();
            lblStatus = new Label();
            lblRole = new Label();
            txtTelefono = new TextBox();
            txtEmail = new TextBox();
            lblPhone = new Label();
            lblEmail = new Label();
            txtApellido = new TextBox();
            lblFullName = new Label();
            txtContraseña = new TextBox();
            lblPassword = new Label();
            txtUsuario = new TextBox();
            lblUsuario = new Label();
            txtId = new TextBox();
            lblId = new Label();
            dgUsuario = new DataGridView();
            colId = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            colUsername = new DataGridViewTextBoxColumn();
            colFullName = new DataGridViewTextBoxColumn();
            colEmail = new DataGridViewTextBoxColumn();
            colRole = new DataGridViewTextBoxColumn();
            pnlSearch = new Panel();
            btnShowAll = new Button();
            btnSearch = new Button();
            txtSearch = new TextBox();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picUsuario).BeginInit();
            flpButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgUsuario).BeginInit();
            pnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(42, 115, 204);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1257, 50);
            pnlHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Cursor = Cursors.Hand;
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnClose.Location = new Point(1207, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(50, 50);
            btnClose.TabIndex = 1;
            btnClose.TabStop = false;
            btnClose.Text = "X";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(16, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(277, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Gestión de Usuarios";
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 50);
            splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(picUsuario);
            splitMain.Panel1.Controls.Add(dtpFechaCreacion);
            splitMain.Panel1.Controls.Add(lblFechaCreacion);
            splitMain.Panel1.Controls.Add(label1);
            splitMain.Panel1.Controls.Add(txtNombre);
            splitMain.Panel1.Controls.Add(flpButtons);
            splitMain.Panel1.Controls.Add(cbxEstado);
            splitMain.Panel1.Controls.Add(cbxRol);
            splitMain.Panel1.Controls.Add(lblStatus);
            splitMain.Panel1.Controls.Add(lblRole);
            splitMain.Panel1.Controls.Add(txtTelefono);
            splitMain.Panel1.Controls.Add(txtEmail);
            splitMain.Panel1.Controls.Add(lblPhone);
            splitMain.Panel1.Controls.Add(lblEmail);
            splitMain.Panel1.Controls.Add(txtApellido);
            splitMain.Panel1.Controls.Add(lblFullName);
            splitMain.Panel1.Controls.Add(txtContraseña);
            splitMain.Panel1.Controls.Add(lblPassword);
            splitMain.Panel1.Controls.Add(txtUsuario);
            splitMain.Panel1.Controls.Add(lblUsuario);
            splitMain.Panel1.Controls.Add(txtId);
            splitMain.Panel1.Controls.Add(lblId);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(dgUsuario);
            splitMain.Panel2.Controls.Add(pnlSearch);
            splitMain.Size = new Size(1257, 588);
            splitMain.SplitterDistance = 595;
            splitMain.TabIndex = 1;
            // 
            // picUsuario
            // 
            picUsuario.Location = new Point(390, 45);
            picUsuario.Name = "picUsuario";
            picUsuario.Size = new Size(186, 163);
            picUsuario.TabIndex = 22;
            picUsuario.TabStop = false;
            // 
            // dtpFechaCreacion
            // 
            dtpFechaCreacion.Location = new Point(214, 469);
            dtpFechaCreacion.Name = "dtpFechaCreacion";
            dtpFechaCreacion.Size = new Size(300, 31);
            dtpFechaCreacion.TabIndex = 21;
            // 
            // lblFechaCreacion
            // 
            lblFechaCreacion.AutoSize = true;
            lblFechaCreacion.ForeColor = Color.Black;
            lblFechaCreacion.Location = new Point(16, 469);
            lblFechaCreacion.Name = "lblFechaCreacion";
            lblFechaCreacion.Size = new Size(152, 25);
            lblFechaCreacion.TabIndex = 20;
            lblFechaCreacion.Text = "Fecha de creación";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 180);
            label1.Name = "label1";
            label1.Size = new Size(91, 25);
            label1.TabIndex = 19;
            label1.Text = "Nombres ";
            // 
            // txtNombre
            // 
            txtNombre.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtNombre.Location = new Point(119, 177);
            txtNombre.Margin = new Padding(3, 6, 3, 6);
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(237, 31);
            txtNombre.TabIndex = 18;
            // 
            // flpButtons
            // 
            flpButtons.Controls.Add(btnNuevo);
            flpButtons.Controls.Add(btnGuardar);
            flpButtons.Controls.Add(btnActualizar);
            flpButtons.Controls.Add(btnEliminar);
            flpButtons.Dock = DockStyle.Bottom;
            flpButtons.Location = new Point(0, 528);
            flpButtons.Name = "flpButtons";
            flpButtons.Size = new Size(595, 60);
            flpButtons.TabIndex = 17;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.FromArgb(42, 115, 204);
            btnNuevo.FlatStyle = FlatStyle.Flat;
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Location = new Point(3, 3);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(110, 36);
            btnNuevo.TabIndex = 0;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(42, 115, 204);
            btnGuardar.FlatStyle = FlatStyle.Flat;
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Location = new Point(119, 3);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(110, 36);
            btnGuardar.TabIndex = 1;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = false;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.FromArgb(42, 115, 204);
            btnActualizar.Enabled = false;
            btnActualizar.FlatStyle = FlatStyle.Flat;
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Location = new Point(235, 3);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(110, 36);
            btnActualizar.TabIndex = 2;
            btnActualizar.Text = "Actualizar";
            btnActualizar.UseVisualStyleBackColor = false;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(42, 115, 204);
            btnEliminar.Enabled = false;
            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Location = new Point(351, 3);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(110, 36);
            btnEliminar.TabIndex = 3;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = false;
            // 
            // cbxEstado
            // 
            cbxEstado.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbxEstado.FormattingEnabled = true;
            cbxEstado.Items.AddRange(new object[] { "Activo", "Inactivo" });
            cbxEstado.Location = new Point(119, 400);
            cbxEstado.Margin = new Padding(3, 6, 3, 6);
            cbxEstado.Name = "cbxEstado";
            cbxEstado.Size = new Size(237, 33);
            cbxEstado.TabIndex = 16;
            // 
            // cbxRol
            // 
            cbxRol.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cbxRol.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxRol.FormattingEnabled = true;
            cbxRol.Location = new Point(119, 355);
            cbxRol.Margin = new Padding(3, 6, 3, 6);
            cbxRol.Name = "cbxRol";
            cbxRol.Size = new Size(237, 33);
            cbxRol.TabIndex = 15;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(33, 405);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(66, 25);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Estado";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Location = new Point(54, 360);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(37, 25);
            lblRole.TabIndex = 11;
            lblRole.Text = "Rol";
            // 
            // txtTelefono
            // 
            txtTelefono.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtTelefono.Location = new Point(119, 312);
            txtTelefono.Margin = new Padding(3, 6, 3, 6);
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(237, 31);
            txtTelefono.TabIndex = 10;
            // 
            // txtEmail
            // 
            txtEmail.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEmail.Location = new Point(119, 260);
            txtEmail.Margin = new Padding(3, 6, 3, 6);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(237, 31);
            txtEmail.TabIndex = 9;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(16, 320);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(79, 25);
            lblPhone.TabIndex = 8;
            lblPhone.Text = "Teléfono";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(41, 281);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(54, 25);
            lblEmail.TabIndex = 7;
            lblEmail.Text = "Email";
            // 
            // txtApellido
            // 
            txtApellido.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtApellido.Location = new Point(119, 217);
            txtApellido.Margin = new Padding(3, 6, 3, 6);
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(237, 31);
            txtApellido.TabIndex = 6;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Location = new Point(13, 227);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(86, 25);
            lblFullName.TabIndex = 5;
            lblFullName.Text = "Apellidos";
            // 
            // txtContraseña
            // 
            txtContraseña.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtContraseña.Location = new Point(119, 132);
            txtContraseña.Margin = new Padding(3, 6, 3, 6);
            txtContraseña.Name = "txtContraseña";
            txtContraseña.Size = new Size(237, 31);
            txtContraseña.TabIndex = 4;
            txtContraseña.UseSystemPasswordChar = true;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(12, 135);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(101, 25);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Contraseña";
            // 
            // txtUsuario
            // 
            txtUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtUsuario.Location = new Point(119, 88);
            txtUsuario.Margin = new Padding(3, 6, 3, 6);
            txtUsuario.Name = "txtUsuario";
            txtUsuario.Size = new Size(237, 31);
            txtUsuario.TabIndex = 0;
            // 
            // lblUsuario
            // 
            lblUsuario.AutoSize = true;
            lblUsuario.Location = new Point(37, 85);
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(72, 25);
            lblUsuario.TabIndex = 2;
            lblUsuario.Text = "Usuario";
            // 
            // txtId
            // 
            txtId.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtId.Location = new Point(119, 45);
            txtId.Margin = new Padding(3, 6, 3, 6);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(237, 31);
            txtId.TabIndex = 1;
            // 
            // lblId
            // 
            lblId.AutoSize = true;
            lblId.Location = new Point(68, 39);
            lblId.Name = "lblId";
            lblId.Size = new Size(30, 25);
            lblId.TabIndex = 0;
            lblId.Text = "ID";
            // 
            // dgUsuario
            // 
            dgUsuario.AllowUserToAddRows = false;
            dgUsuario.AllowUserToDeleteRows = false;
            dgUsuario.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgUsuario.BackgroundColor = Color.White;
            dgUsuario.BorderStyle = BorderStyle.None;
            dgUsuario.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgUsuario.Columns.AddRange(new DataGridViewColumn[] { colId, colStatus, colUsername, colFullName, colEmail, colRole });
            dgUsuario.Dock = DockStyle.Fill;
            dgUsuario.Location = new Point(0, 50);
            dgUsuario.Name = "dgUsuario";
            dgUsuario.ReadOnly = true;
            dgUsuario.RowHeadersVisible = false;
            dgUsuario.RowHeadersWidth = 62;
            dgUsuario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgUsuario.Size = new Size(658, 538);
            dgUsuario.TabIndex = 1;
            // 
            // colId
            // 
            colId.DataPropertyName = "Id";
            colId.HeaderText = "ID";
            colId.MinimumWidth = 8;
            colId.Name = "colId";
            colId.ReadOnly = true;
            colId.Visible = false;
            // 
            // colStatus
            // 
            colStatus.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colStatus.DataPropertyName = "Status";
            colStatus.HeaderText = "Estado";
            colStatus.MinimumWidth = 8;
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 102;
            // 
            // colUsername
            // 
            colUsername.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colUsername.DataPropertyName = "Username";
            colUsername.HeaderText = "Username";
            colUsername.MinimumWidth = 8;
            colUsername.Name = "colUsername";
            colUsername.ReadOnly = true;
            colUsername.Width = 127;
            // 
            // colFullName
            // 
            colFullName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colFullName.DataPropertyName = "FullName";
            colFullName.HeaderText = "Nombre Completo";
            colFullName.MinimumWidth = 8;
            colFullName.Name = "colFullName";
            colFullName.ReadOnly = true;
            // 
            // colEmail
            // 
            colEmail.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colEmail.DataPropertyName = "Email";
            colEmail.HeaderText = "Email";
            colEmail.MinimumWidth = 8;
            colEmail.Name = "colEmail";
            colEmail.ReadOnly = true;
            // 
            // colRole
            // 
            colRole.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colRole.DataPropertyName = "Role";
            colRole.HeaderText = "Rol";
            colRole.MinimumWidth = 8;
            colRole.Name = "colRole";
            colRole.ReadOnly = true;
            colRole.Width = 73;
            // 
            // pnlSearch
            // 
            pnlSearch.Controls.Add(btnShowAll);
            pnlSearch.Controls.Add(btnSearch);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(658, 50);
            pnlSearch.TabIndex = 0;
            // 
            // btnShowAll
            // 
            btnShowAll.BackColor = Color.FromArgb(42, 115, 204);
            btnShowAll.FlatStyle = FlatStyle.Flat;
            btnShowAll.ForeColor = Color.White;
            btnShowAll.Location = new Point(310, 10);
            btnShowAll.Name = "btnShowAll";
            btnShowAll.Size = new Size(110, 30);
            btnShowAll.TabIndex = 2;
            btnShowAll.Text = "Mostrar todo";
            btnShowAll.UseVisualStyleBackColor = false;
            // 
            // btnSearch
            // 
            btnSearch.BackColor = Color.FromArgb(42, 115, 204);
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.ForeColor = Color.White;
            btnSearch.Location = new Point(220, 10);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(80, 30);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Buscar";
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += btnSearch_Click;
            // 
            // txtSearch
            // 
            txtSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(10, 12);
            txtSearch.Margin = new Padding(3, 6, 3, 6);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Buscar...";
            txtSearch.Size = new Size(212, 34);
            txtSearch.TabIndex = 0;
            // 
            // UsuariosUI
            // 
            ClientSize = new Size(1257, 638);
            Controls.Add(splitMain);
            Controls.Add(pnlHeader);
            Name = "UsuariosUI";
            Load += UsuarioUI_Load;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel1.PerformLayout();
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picUsuario).EndInit();
            flpButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgUsuario).EndInit();
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ResumeLayout(false);

        }

        private void txtUserNameKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) SaveData();
        }
        private Panel pnlHeader;
        private Label lblTitle;
        private Button btnClose;
        private SplitContainer splitMain;
        private TextBox txtUsuario;
        private Label lblUsuario;
        private TextBox txtId;
        private Label lblId;
        private Label lblFullName;
        private TextBox txtContraseña;
        private Label lblPassword;
        private TextBox txtTelefono;
        private TextBox txtEmail;
        private Label lblPhone;
        private Label lblEmail;
        private TextBox txtApellido;
        private ComboBox cbxRol;
        private Label lblStatus;
        private Label lblRole;
        private ComboBox cbxEstado;
        private FlowLayoutPanel flpButtons;
        private Button btnNuevo;
        private Button btnGuardar;
        private Button btnActualizar;
        private Button btnEliminar;
        private TextBox txtSearch;
        private Button btnShowAll;
        private Button btnSearch;
        private DataGridView dgUsuario;
        private DataGridViewTextBoxColumn colId;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn colUsername;
        private DataGridViewTextBoxColumn colFullName;
        private DataGridViewTextBoxColumn colEmail;
        private DataGridViewTextBoxColumn colRole;
        private Label label1;
        private TextBox txtNombre;
        private Label lblFechaCreacion;
        private DateTimePicker dtpFechaCreacion;
        private PictureBox picUsuario;
        private Panel pnlSearch;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UsuarioUI_Load(object sender, EventArgs e)
        {
            // Estilo DataGrid
            ApplyGridStyle(dgUsuario);

            // Cargar opciones
            cbxRol.Items.AddRange(new string[] { "Administrador", "Usuario" });
            cbxEstado.Items.AddRange(new string[] { "Activo", "Inactivo" });
            cbxEstado.SelectedIndex = 0;

            // Cargar datos — usar los métodos ya existentes en este archivo
            FillGrid();
            ClearData();
        }

        private void ApplyGridStyle(DataGridView dg)
        {
            dg.BorderStyle = BorderStyle.None;
            dg.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);
            dg.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dg.DefaultCellStyle.SelectionBackColor = Color.FromArgb(42, 115, 204);
            dg.DefaultCellStyle.SelectionForeColor = Color.White;
            dg.BackgroundColor = Color.White;
            dg.EnableHeadersVisualStyles = false;
            dg.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 115, 204);
            dg.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dg.ColumnHeadersHeight = 35;
            dg.RowTemplate.Height = 28;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            ClearData();
        }
    }
}

