namespace KooliProjekt.WindowsForms;

partial class Form1
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
        components = new System.ComponentModel.Container();
        dataGridView1 = new DataGridView();
        btnAdd = new Button();
        btnDelete = new Button();
        ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
        SuspendLayout();
        // 
        // dataGridView1
        // 
        dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridView1.Location = new Point(12, 12);
        dataGridView1.Name = "dataGridView1";
        dataGridView1.Size = new Size(776, 380);
        dataGridView1.TabIndex = 0;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        
        // 
        // btnAdd
        // 
        btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnAdd.Location = new Point(600, 405);
        btnAdd.Name = "btnAdd";
        btnAdd.Size = new Size(90, 30);
        btnAdd.TabIndex = 1;
        btnAdd.Text = "Add New";
        btnAdd.UseVisualStyleBackColor = true;
        btnAdd.Click += btnAdd_Click;
        
        // 
        // btnDelete
        // 
        btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDelete.Location = new Point(698, 405);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(90, 30);
        btnDelete.TabIndex = 2;
        btnDelete.Text = "Delete";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;

        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(btnDelete);
        Controls.Add(btnAdd);
        Controls.Add(dataGridView1);
        Name = "Form1";
        Text = "KooliProjekt Users";
        Load += Form1_Load;
        ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView dataGridView1;
    private Button btnAdd;
    private Button btnDelete;
}
