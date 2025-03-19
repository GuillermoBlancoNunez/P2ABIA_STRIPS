// Formulario principal con la interfaz mejorada.
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
    private List<MoveAction> actionsToExecute;
    private int currentActionIndex = 0;

    // Para permitir retroceder se guarda el historial de estados.
    private WorldState initialWorldState;
    private List<WorldState> stateHistory;

    public MainForm()
    {
        this.Text = "Mundo de Bloques - Agente STRIPS";
        this.Size = new Size(1000, 700);
        this.BackColor = Color.WhiteSmoke;

        // Título de la aplicación.
        this.titleLabel = new Label();
        this.titleLabel.Text = "MUNDO DE BLOQUES - AGENTE STRIPS";
        this.titleLabel.Font = new Font("Segoe UI", 20, FontStyle.Bold);
        this.titleLabel.ForeColor = Color.DarkBlue;
        this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
        this.titleLabel.Dock = DockStyle.Top;
        this.titleLabel.Height = 60;
        this.Controls.Add(this.titleLabel);

        // GroupBox para mostrar el plan de acciones.
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

        // Botón para iniciar la ejecución.
        this.startButton = new Button();
        this.startButton.Text = "Iniciar Ejecución";
        this.startButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.startButton.Size = new Size(150, 40);
        this.startButton.Location = new Point(20, 340);
        this.startButton.BackColor = Color.LightGreen;
        this.startButton.Click += new EventHandler(this.StartButton_Click);
        this.Controls.Add(this.startButton);

        // Botón para la siguiente acción.
        this.nextActionButton = new Button();
        this.nextActionButton.Text = "Siguiente Acción";
        this.nextActionButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.nextActionButton.Size = new Size(150, 40);
        this.nextActionButton.Location = new Point(200, 340);
        this.nextActionButton.BackColor = Color.LightSkyBlue;
        this.nextActionButton.Enabled = false;
        this.nextActionButton.Click += new EventHandler(this.NextActionButton_Click);
        this.Controls.Add(this.nextActionButton);

        // Botón para reiniciar (colocado debajo del botón Iniciar Ejecución).
        this.resetButton = new Button();
        this.resetButton.Text = "Reiniciar";
        this.resetButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.resetButton.Size = new Size(150, 40);
        this.resetButton.Location = new Point(20, 390);
        this.resetButton.BackColor = Color.Khaki;
        this.resetButton.Click += new EventHandler(this.ResetButton_Click);
        this.Controls.Add(this.resetButton);

        // Botón para la acción anterior (colocado debajo del botón Siguiente Acción).
        this.previousActionButton = new Button();
        this.previousActionButton.Text = "Acción Anterior";
        this.previousActionButton.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        this.previousActionButton.Size = new Size(150, 40);
        this.previousActionButton.Location = new Point(200, 390);
        this.previousActionButton.BackColor = Color.LightPink;
        this.previousActionButton.Enabled = false;
        this.previousActionButton.Click += new EventHandler(this.PreviousActionButton_Click);
        this.Controls.Add(this.previousActionButton);

        // Label de estado, ubicado en la parte inferior.
        this.statusLabel = new Label();
        this.statusLabel.Text = "Estado: Esperando inicio...";
        this.statusLabel.Font = new Font("Segoe UI", 10, FontStyle.Italic);
        this.statusLabel.AutoSize = true;
        this.statusLabel.Location = new Point(20, 450);
        this.Controls.Add(this.statusLabel);

        // Panel para la representación gráfica.
        this.drawingPanel = new Panel();
        this.drawingPanel.Location = new Point(350, 80);
        this.drawingPanel.Size = new Size(600, 550);
        this.drawingPanel.BackColor = Color.White;
        this.drawingPanel.BorderStyle = BorderStyle.FixedSingle;
        this.drawingPanel.Paint += new PaintEventHandler(this.DrawingPanel_Paint);
        this.Controls.Add(this.drawingPanel);

        WorldState initialState = Scenario.GetInitialState();
        WorldState goalState = Scenario.GetGoalState();

        this.agent = new Agent(initialState, goalState);
        // Guardamos el estado inicial y preparamos el historial.
        this.initialWorldState = initialState.Clone();
        this.stateHistory = new List<WorldState>();
        this.stateHistory.Add(this.initialWorldState);

        // Mostrar el plan completo.
        if (this.agent.Plan != null)
        {
            foreach (MoveAction act in this.agent.Plan)
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
        this.actionsToExecute = new List<MoveAction>(this.agent.Plan);
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
            MoveAction action = this.actionsToExecute[this.currentActionIndex];
            try
            {
                this.agent.CurrentState = action.Apply(this.agent.CurrentState);
                this.currentActionIndex = this.currentActionIndex + 1;
                // Guardar el nuevo estado en el historial.
                this.stateHistory.Add(this.agent.CurrentState.Clone());
                // Resaltar la acción ejecutada.
                this.planListBox.SelectedIndex = this.currentActionIndex - 1;
                this.previousActionButton.Enabled = (this.currentActionIndex > 0);
                this.statusLabel.Text = "Estado: Ejecutada acción: " + action.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error durante la ejecución del plan: " + ex.Message);
            }
            // Actualizar la representación gráfica inmediatamente.
            this.drawingPanel.Invalidate();
            this.drawingPanel.Refresh();
            
            if (this.currentActionIndex >= this.actionsToExecute.Count)
            {
                // Una vez actualizado el panel, mostrar el mensaje pop-up.
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
            this.currentActionIndex = this.currentActionIndex - 1;
            this.agent.CurrentState = this.stateHistory[this.currentActionIndex].Clone();
            if (this.currentActionIndex > 0)
            {
                this.planListBox.SelectedIndex = this.currentActionIndex - 1;
            }
            else
            {
                this.planListBox.ClearSelected();
            }
            this.nextActionButton.Enabled = true;
            this.statusLabel.Text = "Estado: Retrocedida acción. Índice: " + this.currentActionIndex;
            this.stateHistory.RemoveAt(this.stateHistory.Count - 1);
            this.drawingPanel.Invalidate();
            if (this.currentActionIndex == 0)
            {
                this.previousActionButton.Enabled = false;
            }
        }
    }

    private void ResetButton_Click(object sender, EventArgs e)
    {
        this.agent.CurrentState = this.initialWorldState.Clone();
        this.currentActionIndex = 0;
        this.stateHistory.Clear();
        this.stateHistory.Add(this.initialWorldState.Clone());
        this.planListBox.ClearSelected();
        this.nextActionButton.Enabled = true;
        this.previousActionButton.Enabled = false;
        this.startButton.Enabled = true;
        this.statusLabel.Text = "Estado: Reiniciado al estado inicial.";
        this.drawingPanel.Invalidate();
    }

    // Dibuja gráficamente el estado actual, ubicando las pilas según las posiciones fijas T1, T2 y T3.
    private void DrawingPanel_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.Clear(Color.White);

        // Definir las posiciones fijas de la mesa.
        List<string> tablePositions = new List<string>() { "T1", "T2", "T3" };
        Dictionary<string, List<string>> stacks = new Dictionary<string, List<string>>();
        foreach (string tp in tablePositions)
        {
            stacks[tp] = new List<string>();
        }

        // Construir las pilas: buscar el bloque cuya posición de soporte es una de las posiciones de la mesa.
        foreach (string block in this.agent.CurrentState.Positions.Keys)
        {
            string support = this.agent.CurrentState.Positions[block];
            if (MoveAction.IsTablePosition(support))
            {
                if (stacks[support].Count == 0)
                {
                    stacks[support].Add(block);
                    string current = block;
                    bool found = true;
                    while (found)
                    {
                        found = false;
                        foreach (string b in this.agent.CurrentState.Positions.Keys)
                        {
                            if (!stacks[support].Contains(b) && this.agent.CurrentState.Positions[b] == current)
                            {
                                stacks[support].Add(b);
                                current = b;
                                found = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        // Coordenadas fijas para cada posición.
        int panelWidth = this.drawingPanel.Width;
        int margin = 40;
        int blockWidth = 60;
        int blockHeight = 30;
        int t1_x = margin;
        int t3_x = panelWidth - blockWidth - margin;
        int t2_x = (t1_x + t3_x) / 2;
        Dictionary<string, int> tableX = new Dictionary<string, int>()
        {
            { "T1", t1_x },
            { "T2", t2_x },
            { "T3", t3_x }
        };

        int startY = this.drawingPanel.Height - 70;

        // Dibujar cada pila en su posición.
        foreach (string tp in tablePositions)
        {
            int x = tableX[tp];
            int y = startY;
            List<string> stack = stacks[tp];
            if (stack.Count > 0)
            {
                foreach (string block in stack)
                {
                    Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                    g.FillRectangle(Brushes.LightCoral, rect);
                    g.DrawRectangle(Pens.DarkRed, rect);
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;
                    g.DrawString(block, new Font("Segoe UI", 10), Brushes.Black, rect, sf);
                    y -= blockHeight;
                }
            }
            else
            {
                Rectangle rect = new Rectangle(x, y - blockHeight, blockWidth, blockHeight);
                g.DrawRectangle(Pens.Gray, rect);
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(tp, new Font("Segoe UI", 10, FontStyle.Italic), Brushes.Gray, rect, sf);
            }
        }
    }
}
