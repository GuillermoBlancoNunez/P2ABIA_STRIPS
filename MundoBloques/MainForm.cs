using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class MainForm : Form
{
    // Controles de la interfaz
    private Panel drawingPanel;
    private ListBox planListBox;
    private Button btnStart, btnNext, btnPrevious, btnReset;
    private Label lblTitle, lblStatus;

    // Campos del planificador STRIPS
    private Agent agent;
    private List<StripsAction> actions;
    private int actionIndex;
    private HashSet<Predicate> initialState;
    private List<HashSet<Predicate>> stateHistory;

    public MainForm()
    {
        InitializeUI();
        InitializeAgent();
    }

    private void InitializeUI()
    {
        this.Text = "Mundo de Bloques STRIPS";
        this.Size = new Size(1000, 800);
        this.BackColor = Color.WhiteSmoke;

        // Título
        lblTitle = new Label();
        lblTitle.Text = "MUNDO DE BLOQUES - STRIPS";
        lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        lblTitle.ForeColor = Color.DarkBlue;
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Height = 60;
        this.Controls.Add(lblTitle);

        // Grupo para mostrar el plan de acciones
        GroupBox gbPlan = new GroupBox();
        gbPlan.Text = "Plan de Acciones";
        gbPlan.Font = new Font("Segoe UI", 10);
        gbPlan.ForeColor = Color.DarkGreen;
        gbPlan.Size = new Size(300, 250);
        gbPlan.Location = new Point(20, 80);
        this.Controls.Add(gbPlan);

        planListBox = new ListBox();
        planListBox.Font = new Font("Segoe UI", 10);
        planListBox.Location = new Point(10, 25);
        planListBox.Size = new Size(280, 200);
        gbPlan.Controls.Add(planListBox);

        // Botones de control
        btnStart = CreateButton("Iniciar Ejecución", new Point(20, 340), BtnStart_Click);
        btnNext = CreateButton("Siguiente Acción", new Point(200, 340), BtnNext_Click, false);
        btnReset = CreateButton("Reiniciar", new Point(20, 390), BtnReset_Click);
        btnPrevious = CreateButton("Acción Anterior", new Point(200, 390), BtnPrevious_Click, false);
        this.Controls.Add(btnStart);
        this.Controls.Add(btnNext);
        this.Controls.Add(btnReset);
        this.Controls.Add(btnPrevious);

        // Etiqueta de estado
        lblStatus = new Label();
        lblStatus.Text = "Estado: Esperando inicio...";
        lblStatus.Font = new Font("Segoe UI", 10, FontStyle.Italic);
        lblStatus.AutoSize = true;
        lblStatus.Location = new Point(20, 650);
        this.Controls.Add(lblStatus);

        // Panel de dibujo para mostrar el estado del mundo
        drawingPanel = new Panel();
        drawingPanel.Location = new Point(350, 80);
        drawingPanel.Size = new Size(600, 550);
        drawingPanel.BackColor = Color.White;
        drawingPanel.BorderStyle = BorderStyle.FixedSingle;
        drawingPanel.Paint += DrawingPanel_Paint;
        this.Controls.Add(drawingPanel);
    }

    private Button CreateButton(string text, Point location, EventHandler clickHandler, bool enabled = true)
    {
        Button btn = new Button();
        btn.Text = text;
        btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        btn.Size = new Size(150, 40);
        btn.Location = location;
        btn.Enabled = enabled;
        btn.Click += clickHandler;
        return btn;
    }

    private void InitializeAgent()
    {
        initialState = Scenario.GetInitialState();
        HashSet<Predicate> goalState = Scenario.GetGoalState();
        agent = new Agent(initialState, goalState);
        stateHistory = new List<HashSet<Predicate>> { new HashSet<Predicate>(initialState) };

        actions = agent.Plan != null ? new List<StripsAction>(agent.Plan) : new List<StripsAction>();

        if (actions.Count > 0)
        {
            foreach (var action in actions)
                planListBox.Items.Add(action.ToString());
        }
        else
        {
            planListBox.Items.Add("No se encontró un plan.");
        }
    }

    private void BtnStart_Click(object sender, EventArgs e)
    {
        if (actions.Count == 0)
        {
            MessageBox.Show("No se encontró un plan para el escenario.");
            return;
        }
        actionIndex = 0;
        btnStart.Enabled = false;
        btnNext.Enabled = true;
        btnPrevious.Enabled = false;
        lblStatus.Text = "Estado: Ejecución iniciada.";
    }

    private void BtnNext_Click(object sender, EventArgs e)
    {
        if (actionIndex < actions.Count)
        {
            StripsAction action = actions[actionIndex];
            try
            {
                // Aplicar acción y guardar el nuevo estado
                agent.CurrentState = action.Apply(agent.CurrentState);
                stateHistory.Add(new HashSet<Predicate>(agent.CurrentState));
                planListBox.SelectedIndex = actionIndex;
                actionIndex++;
                lblStatus.Text = "Estado: Ejecutada acción: " + action.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante la ejecución: " + ex.Message);
                return;
            }
            drawingPanel.Invalidate();
            if (actionIndex == actions.Count)
            {
                MessageBox.Show("Plan completado.");
                btnNext.Enabled = false;
                lblStatus.Text = "Estado: Plan completado.";
            }
            btnPrevious.Enabled = actionIndex > 0;
        }
    }

    private void BtnPrevious_Click(object sender, EventArgs e)
    {
        if (actionIndex > 0)
        {
            actionIndex--;
            agent.CurrentState = new HashSet<Predicate>(stateHistory[actionIndex]);
            planListBox.SelectedIndex = actionIndex > 0 ? actionIndex - 1 : -1;
            lblStatus.Text = "Estado: Retrocedida acción. Índice: " + actionIndex;
            stateHistory.RemoveAt(stateHistory.Count - 1);
            drawingPanel.Invalidate();
            btnNext.Enabled = true;
            btnPrevious.Enabled = actionIndex > 0;
        }
    }

    private void BtnReset_Click(object sender, EventArgs e)
    {
        agent.CurrentState = new HashSet<Predicate>(initialState);
        actionIndex = 0;
        stateHistory.Clear();
        stateHistory.Add(new HashSet<Predicate>(initialState));
        planListBox.ClearSelected();
        btnNext.Enabled = true;
        btnPrevious.Enabled = false;
        btnStart.Enabled = true;
        lblStatus.Text = "Estado: Reiniciado al estado inicial.";
        drawingPanel.Invalidate();
    }

    private void DrawingPanel_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.Clear(Color.White);

        // Posiciones fijas de la mesa
        string[] tables = { "T1", "T2", "T3" };
        // Construir las pilas de bloques para cada posición
        Dictionary<string, List<string>> stacks = new Dictionary<string, List<string>>();
        foreach (var table in tables)
            stacks[table] = new List<string>();

        foreach (Predicate pred in agent.CurrentState)
        {
            if (pred.Name == "Encima")
            {
                string block = pred.Arguments[0];
                string basePos = pred.Arguments[1];
                if (tables.Contains(basePos))
                {
                    if (!stacks[basePos].Contains(block))
                        stacks[basePos].Add(block);
                    // Buscar bloques que estén encima en la pila
                    string current = block;
                    bool found = true;
                    while (found)
                    {
                        found = false;
                        foreach (var p in agent.CurrentState)
                        {
                            if (p.Name == "Encima" && p.Arguments[1] == current)
                            {
                                stacks[basePos].Add(p.Arguments[0]);
                                current = p.Arguments[0];
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Configuración de coordenadas para dibujar
        int panelWidth = drawingPanel.Width;
        int blockWidth = 60;
        int blockHeight = 30;
        int margin = 20;
        int startY = drawingPanel.Height - 50;

        int t1_x = margin;
        int t3_x = panelWidth - blockWidth - margin;
        int t2_x = (t1_x + t3_x) / 2;
        Dictionary<string, int> tableX = new Dictionary<string, int>()
        {
            { "T1", t1_x },
            { "T2", t2_x },
            { "T3", t3_x }
        };

        // Dibujar cada posición y sus bloques
        foreach (string table in tables)
        {
            int x = tableX[table];
            int y = startY;
            var stack = stacks[table];
            if (stack.Count == 0)
            {
                Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                g.DrawRectangle(Pens.Gray, rect);
                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString(table, this.Font, Brushes.Gray, rect, sf);
            }
            else
            {
                foreach (string block in stack)
                {
                    Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                    g.FillRectangle(Brushes.LightCoral, rect);
                    g.DrawRectangle(Pens.DarkRed, rect);
                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(block, this.Font, Brushes.Black, rect, sf);
                    y -= blockHeight;
                }
            }
        }
    }
}
