namespace KooliProjekt.WindowsForms;

public partial class Form1: Form
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        dataGridView1 = new DataGridView();
        idField = new TextBox();
        firstNameField = new TextBox();
        lastNameField = new TextBox();
        emailField = new TextBox();
        phoneField = new TextBox();
        saveCommand = new Button();
        addCommand = new Button();
        labelId = new Label();
        labelFirstName = new Label();
        labelLastName = new Label();
        labelEmail = new Label();
        labelPhone = new Label();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new Point(12, 12);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new Size(760, 290);
        dataGridView1.TabIndex = 0;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        // 
        // labelId
        // 
        labelId.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelId.AutoSize = true;
        labelId.Location = new Point(12, 320);
        labelId.Name = "labelId";
        labelId.Size = new Size(21, 15);
        labelId.TabIndex = 1;
        labelId.Text = "ID:";
        // 
        // idField
        // 
        idField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        idField.Location = new Point(90, 317);
        idField.Name = "idField";
        idField.ReadOnly = true;
        idField.Size = new Size(100, 23);
        idField.TabIndex = 2;
        // 
        // labelFirstName
        // 
        labelFirstName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelFirstName.AutoSize = true;
        labelFirstName.Location = new Point(12, 350);
        labelFirstName.Name = "labelFirstName";
        labelFirstName.Size = new Size(67, 15);
        labelFirstName.TabIndex = 3;
        labelFirstName.Text = "First Name:";
        // 
        // firstNameField
        // 
        firstNameField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        firstNameField.Location = new Point(90, 347);
        firstNameField.Name = "firstNameField";
        firstNameField.Size = new Size(682, 23);
        firstNameField.TabIndex = 4;
        // 
        // labelLastName
        // 
        labelLastName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelLastName.AutoSize = true;
        labelLastName.Location = new Point(12, 380);
        labelLastName.Name = "labelLastName";
        labelLastName.Size = new Size(66, 15);
        labelLastName.TabIndex = 5;
        labelLastName.Text = "Last Name:";
        // 
        // lastNameField
        // 
        lastNameField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lastNameField.Location = new Point(90, 377);
        lastNameField.Name = "lastNameField";
        lastNameField.Size = new Size(682, 23);
        lastNameField.TabIndex = 6;
        // 
        // labelEmail
        // 
        labelEmail.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelEmail.AutoSize = true;
        labelEmail.Location = new Point(12, 410);
        labelEmail.Name = "labelEmail";
        labelEmail.Size = new Size(39, 15);
        labelEmail.TabIndex = 7;
        labelEmail.Text = "Email:";
        // 
        // emailField
        // 
        emailField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        emailField.Location = new Point(90, 407);
        emailField.Name = "emailField";
        emailField.Size = new Size(682, 23);
        emailField.TabIndex = 8;
        // 
        // labelPhone
        // 
        labelPhone.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        labelPhone.AutoSize = true;
        labelPhone.Location = new Point(12, 440);
        labelPhone.Name = "labelPhone";
        labelPhone.Size = new Size(44, 15);
        labelPhone.TabIndex = 9;
        labelPhone.Text = "Phone:";
        // 
        // phoneField
        // 
        phoneField.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        phoneField.Location = new Point(90, 437);
        phoneField.Name = "phoneField";
        phoneField.Size = new Size(682, 23);
        phoneField.TabIndex = 10;
        // 
        // saveCommand
        // 
        saveCommand.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveCommand.Location = new Point(586, 480);
        saveCommand.Name = "saveCommand";
        saveCommand.Size = new Size(90, 30);
        saveCommand.TabIndex = 11;
        saveCommand.Text = "Save";
        saveCommand.UseVisualStyleBackColor = true;
        // 
        // addCommand
        // 
        addCommand.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        addCommand.Location = new Point(682, 480);
        addCommand.Name = "addCommand";
        addCommand.Size = new Size(90, 30);
        addCommand.TabIndex = 12;
        addCommand.Text = "Add New";
        addCommand.UseVisualStyleBackColor = true;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 531);
        Controls.Add(addCommand);
        Controls.Add(saveCommand);
        Controls.Add(phoneField);
        Controls.Add(labelPhone);
        Controls.Add(emailField);
        Controls.Add(labelEmail);
        Controls.Add(lastNameField);
        Controls.Add(labelLastName);
        Controls.Add(firstNameField);
        Controls.Add(labelFirstName);
        Controls.Add(idField);
        Controls.Add(labelId);
        Controls.Add(dataGridView1);
        Name = "Form1";
        Text = "KooliProjekt Users";
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView dataGridView1;
    private TextBox idField;
    private TextBox firstNameField;
    private TextBox lastNameField;
    private TextBox emailField;
    private TextBox phoneField;
    private Button saveCommand;
    private Button addCommand;
    private Label labelId;
    private Label labelFirstName;
    private Label labelLastName;
    private Label labelEmail;
    private Label labelPhone;
}
