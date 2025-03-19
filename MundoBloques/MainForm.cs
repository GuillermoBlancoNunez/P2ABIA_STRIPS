using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


public class MainForm : Form
{
    private Panel drawingPanel;
    private ListBox planListBox;
    private Button startButton;
    private Button nextActionButton;
    private Button previousActionButton;
    private Button resetButton;
    private Label titleLabel;
    private Label statusLabel;
    private Agent agent;
    private List<StripsAction> actionsToExecute;
    private int currentActionIndex = 0;
    private HashSet<Predicate> initialWorldState;
    private List<HashSet<Predicate>> stateHistory;

    public MainForm()
    {
        this.Text = "Mundo de Bloques STRIPS";
        this.Size = new Size(1000, 800);
        this.BackColor = Color.WhiteSmoke;

        // Título
        this.titleLabel = new Label();
        this.titleLabel.Text = "MUNDO DE BLOQUES - STRIPS";
        this.titleLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        this.titleLabel.ForeColor = Color.DarkBlue;
        this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.titleLabel.Dock = DockStyle.Top;
        this.titleLabel.Height = 60;
        this.Controls.Add(this.titleLabel);

        // GroupBox para el plan
        GroupBox planGroupBox = new GroupBox();
        planGroupBox.Text = "Plan de Acciones";
        planGroupBox.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        planGroupBox.ForeColor = Color.DarkGreen;
        planGroupBox.Size = new Size(300, 250);
        planGroupBox.Location = new Point(20, 80);
        this.Controls.Add(planGroupBox);

        this.planListBox = new ListBox();
        this.planListBox.Font = new Font("Segoe UI", 10);
        this.planListBox.Location = new Point(10, 25);
        this.planListBox.Size = new Size(280, 200);
        planGroupBox.Controls.Add(this.planListBox);

        // Botones de control
        this.startButton = new Button();
        this.startButton.Text = "Iniciar Ejecución";
        this.startButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.startButton.Size = new Size(150, 40);
        this.startButton.Location = new Point(20, 340);
        this.startButton.BackColor = Color.LightGreen;
        this.startButton.Click += new EventHandler(this.StartButton_Click);
        this.Controls.Add(this.startButton);

        this.nextActionButton = new Button();
        this.nextActionButton.Text = "Siguiente Acción";
        this.nextActionButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.nextActionButton.Size = new Size(150, 40);
        this.nextActionButton.Location = new Point(200, 340);
        this.nextActionButton.BackColor = Color.LightSkyBlue;
        this.nextActionButton.Enabled = false;
        this.nextActionButton.Click += new EventHandler(this.NextActionButton_Click);
        this.Controls.Add(this.nextActionButton);

        // Botones colocados debajo
        this.resetButton = new Button();
        this.resetButton.Text = "Reiniciar";
        this.resetButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.resetButton.Size = new Size(150, 40);
        this.resetButton.Location = new Point(20, 390);
        this.resetButton.BackColor = Color.Khaki;
        this.resetButton.Click += new EventHandler(this.ResetButton_Click);
        this.Controls.Add(this.resetButton);

        this.previousActionButton = new Button();
        this.previousActionButton.Text = "Acción Anterior";
        this.previousActionButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.previousActionButton.Size = new Size(150, 40);
        this.previousActionButton.Location = new Point(200, 390);
        this.previousActionButton.BackColor = Color.LightPink;
        this.previousActionButton.Enabled = false;
        this.previousActionButton.Click += new EventHandler(this.PreviousActionButton_Click);
        this.Controls.Add(this.previousActionButton);

        // Label de estado en la parte inferior
        this.statusLabel = new Label();
        this.statusLabel.Text = "Estado: Esperando inicio...";
        this.statusLabel.Font = new Font("Segoe UI", 10, FontStyle.Italic);
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new Point(20, 650);
        this.Controls.Add(this.statusLabel);

        // Panel de dibujo
        this.drawingPanel = new Panel();
        this.drawingPanel.Location = new Point(350, 80);
        this.drawingPanel.Size = new Size(600, 550);
        this.drawingPanel.BackColor = Color.White;
        this.drawingPanel.BorderStyle = BorderStyle.FixedSingle;
        this.drawingPanel.Paint += new PaintEventHandler(this.DrawingPanel_Paint);
        this.Controls.Add(this.drawingPanel);

        // Inicialización del agente
        HashSet<Predicate> initialState = Scenario.GetInitialState();
        HashSet<Predicate> goalState = Scenario.GetGoalState();
        this.agent = new Agent(initialState, goalState);

        this.initialWorldState = new HashSet<Predicate>(initialState);
        this.stateHistory = new List<HashSet<Predicate>>();
        this.stateHistory.Add(new HashSet<Predicate>(initialState));

        if (this.agent.Plan != null)
        {
            foreach (var act in this.agent.Plan)
            {
                this.planListBox.Items.Add(act.ToString());
            }
        }
        else
        {
            this.planListBox.Items.Add("No se encontró un plan.");
        }
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (this.agent.Plan == null)
            {
                MessageBox.Show("No se encontró un plan para el escenario.");
                return;
            }
            this.actionsToExecute = new List<StripsAction>(this.agent.Plan);
            this.currentActionIndex = 0;
            this.startButton.Enabled = false;
            this.nextActionButton.Enabled = true;
            this.previousActionButton.Enabled = false;
            this.statusLabel.Text = "Estado: Ejecución iniciada.";
        }

        private void NextActionButton_Click(object sender, EventArgs e)
        {
            if (this.currentActionIndex < this.actionsToExecute.Count)
            {
                StripsAction action = this.actionsToExecute[this.currentActionIndex];
                try
                {
                    this.agent.CurrentState = action.Apply(this.agent.CurrentState);
                    this.currentActionIndex++;
                    this.stateHistory.Add(new HashSet<Predicate>(this.agent.CurrentState));
                    this.planListBox.SelectedIndex = this.currentActionIndex - 1;
                    this.previousActionButton.Enabled = (this.currentActionIndex > 0);
                    this.statusLabel.Text = "Estado: Ejecutada acción: " + action.Name;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error durante la ejecución: " + ex.Message);
                }
                this.drawingPanel.Invalidate();
                this.drawingPanel.Refresh();
                if (this.currentActionIndex >= this.actionsToExecute.Count)
                {
                    MessageBox.Show("Plan completado.");
                    this.nextActionButton.Enabled = false;
                    this.statusLabel.Text = "Estado: Plan completado.";
                }
            }
        }

        private void PreviousActionButton_Click(object sender, EventArgs e)
        {
            if (this.currentActionIndex > 0)
            {
                this.currentActionIndex--;
                this.agent.CurrentState = new HashSet<Predicate>(this.stateHistory[this.currentActionIndex]);
                if (this.currentActionIndex > 0)
                    this.planListBox.SelectedIndex = this.currentActionIndex - 1;
                else
                    this.planListBox.ClearSelected();
                this.nextActionButton.Enabled = true;
                this.statusLabel.Text = "Estado: Retrocedida acción. Índice: " + this.currentActionIndex;
                this.stateHistory.RemoveAt(this.stateHistory.Count - 1);
                this.drawingPanel.Invalidate();
                this.drawingPanel.Refresh();
                if (this.currentActionIndex == 0)
                    this.previousActionButton.Enabled = false;
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            this.agent.CurrentState = new HashSet<Predicate>(this.initialWorldState);
            this.currentActionIndex = 0;
            this.stateHistory.Clear();
            this.stateHistory.Add(new HashSet<Predicate>(this.initialWorldState));
            this.planListBox.ClearSelected();
            this.nextActionButton.Enabled = true;
            this.previousActionButton.Enabled = false;
            this.startButton.Enabled = true;
            this.statusLabel.Text = "Estado: Reiniciado al estado inicial.";
            this.drawingPanel.Invalidate();
            this.drawingPanel.Refresh();
        }

        private List<List<string>> GetStacks(HashSet<Predicate> state)
        {
            List<List<string>> stacks = new List<List<string>>();
            var onMesa = state.Where(p => p.Name == "Encima" && p.Arguments[1] == "Mesa")
                            .Select(p => p.Arguments[0]).ToList();
            HashSet<string> visited = new HashSet<string>();
            foreach (string baseBlock in onMesa)
            {
                if (!visited.Contains(baseBlock))
                {
                    List<string> stack = new List<string>();
                    stack.Add(baseBlock);
                    visited.Add(baseBlock);
                    string current = baseBlock;
                    bool found = true;
                    while (found)
                    {
                        found = false;
                        var next = state.Where(p => p.Name == "Encima" && p.Arguments[1] == current)
                                        .Select(p => p.Arguments[0]).FirstOrDefault();
                        if (next != null && !visited.Contains(next))
                        {
                            stack.Add(next);
                            visited.Add(next);
                            current = next;
                            found = true;
                        }
                    }
                    stacks.Add(stack);
                }
            }
            return stacks;
        }

        

        private void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.White);

            // T1, T2, T3 fijos
            string[] tablePositions = { "T1", "T2", "T3" };
            // Calculamos qué bloque está directamente encima de cada Tn
            // y seguimos la cadena "Encima(x,y)".

            Dictionary<string, List<string>> stacks = new Dictionary<string, List<string>>();
            foreach (string tp in tablePositions)
            {
                stacks[tp] = new List<string>();
            }

            // Buscar qué bloque base está sobre T1, T2, T3
            foreach (Predicate p in this.agent.CurrentState)
            {
                if (p.Name == "Encima")
                {
                    string b = p.Arguments[0];
                    string under = p.Arguments[1];
                    if (under == "T1" || under == "T2" || under == "T3")
                    {
                        // b es la base de la pila en T1, T2 o T3
                        if (!stacks[under].Contains(b))
                            stacks[under].Add(b);

                        // Ahora construimos la pila ascendente
                        string current = b;
                        bool found = true;
                        while (found)
                        {
                            found = false;
                            // Buscar un predicado Encima(x, current)
                            foreach (Predicate p2 in this.agent.CurrentState)
                            {
                                if (p2.Name == "Encima" && p2.Arguments[1] == current)
                                {
                                    string top = p2.Arguments[0];
                                    stacks[under].Add(top);
                                    current = top;
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Coordenadas para T1, T2, T3
            int panelWidth = this.drawingPanel.Width;
            int blockWidth = 60;
            int blockHeight = 30;
            int margin = 20;
            int startY = this.drawingPanel.Height - 50;

            // T1 a la izquierda
            int t1_x = margin;
            // T3 a la derecha
            int t3_x = panelWidth - blockWidth - margin;
            // T2 al centro
            int t2_x = (t1_x + t3_x) / 2;

            Dictionary<string, int> tableX = new Dictionary<string, int>()
            {
                { "T1", t1_x },
                { "T2", t2_x },
                { "T3", t3_x }
            };

            // Dibujar cada posición
            foreach (string tp in tablePositions)
            {
                int x = tableX[tp];
                int y = startY;
                List<string> stack = stacks[tp];
                if (stack.Count == 0)
                {
                    // Posición vacía: dibujar un rectángulo con la etiqueta T1, T2 o T3
                    Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                    g.DrawRectangle(Pens.Gray, rect);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(tp, this.Font, Brushes.Gray, rect, sf);
                }
                else
                {
                    // Dibujar la pila
                    foreach (string block in stack)
                    {
                        Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                        g.FillRectangle(Brushes.LightCoral, rect);
                        g.DrawRectangle(Pens.DarkRed, rect);

                        StringFormat sf = new StringFormat();
                        sf.Alignment = StringAlignment.Center;
                        sf.LineAlignment = StringAlignment.Center;
                        g.DrawString(block, this.Font, Brushes.Black, rect, sf);
                        y -= blockHeight;
                    }
                }
            }
        }
}
