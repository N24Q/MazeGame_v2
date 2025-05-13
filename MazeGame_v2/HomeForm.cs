namespace MazeGame_v2
{
    using System;
    using System.Runtime.Versioning;
    using System.Windows.Forms;
    using System.Drawing;
    using System.IO;
    public partial class HomeForm : Form
    {
        private Button startButton; //Initialising home form components
        private TextBox levelInputBox;
        private Button playButton;
        public static int LevelCount { get; private set; } //storing the level count for later use in game functionality and scoring

        public HomeForm() // Adding controls for start button
        {
            InitializeComponent();
            Text = "Maze Game Home"; // Setting form window text

            startButton = new Button
            {
                Text = "Start Game",
                Width = 100
            };

            startButton.Click += ChooseLevels;
            Controls.Add(startButton);

            // Center button initially
            startButton.Location = new Point(
                (ClientSize.Width - startButton.Width) / 2,
                (ClientSize.Height / 2) + 150
            );

            // Separate resize event for startButton
            this.Resize += (sender, e) =>
            {
                startButton.Location = new Point(
                    (ClientSize.Width - startButton.Width) / 2,
                    (ClientSize.Height / 2) + 100
                );
            };
        }
        private Label instructionLabel;

        //BUTTONS FORMATING FIXED BY CHATGPT BECAUSE I WAS GETTING NULL EXCEPTIONS
        private void ChooseLevels(object sender, EventArgs e) // Method to allow user to choose amount of levels to play
        {
            Controls.Remove(startButton); // Removing the start button

            // Adding label for user input
            instructionLabel = new Label
            {
                Text = "Enter the number of levels you want to play (1-10):",
                AutoSize = true, // Allows manual width setting
                TextAlign = ContentAlignment.MiddleCenter // Centers text within label
            };
            Controls.Add(instructionLabel);

            instructionLabel.Location = new Point(
                ((ClientSize.Width - 100) / 2) - 80, // Estimate width
                ((ClientSize.Height - 30) / 2) + 150);

            // Adding text box for input
            levelInputBox = new TextBox { Width = 50 };
            Controls.Add(levelInputBox);

            levelInputBox.Location = new Point(
                ((ClientSize.Width - 60) / 2) - 26, // Estimate width
                ((ClientSize.Height - 30) / 2) + 165);

            // Adding play button
            playButton = new Button
            {
                Text = "Play",
                Width = 60
            };
            playButton.Click += ValidateAndStartGame;
            Controls.Add(playButton);

            playButton.Location = new Point(
                ((ClientSize.Width - 60) / 2) + 26, // Estimate width
                ((ClientSize.Height - 30) / 2) + 165);
        }

        private void ValidateAndStartGame(object sender, EventArgs e) //Method to validate level input
        {
            if (int.TryParse(levelInputBox.Text.Trim(), out int levels)) //If the text from the levelinputbox can be parsed as an integer, pass it on as 'levels'
            {
                if (levels < 1 || levels > 10) //Limiting amount of levels just so a user doesn't choose a crazy amount
                {
                    MessageBox.Show("Please enter a valid level count (1-10)", "Invalid Input");
                    return;
                }

                LevelCount = levels; //store the chosen amount of levels and start the first game
                Hide();
                GameForm gameForm = new GameForm();
                gameForm.ShowDialog();
                Close();
            }
            else //Otherwise send error message so user has to enter another input
            {
                MessageBox.Show("Please enter a number", "Invalid Input");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
