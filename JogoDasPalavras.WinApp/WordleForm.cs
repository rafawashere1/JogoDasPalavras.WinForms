﻿using JogoDasPalavras.WinApp.Shared;

namespace JogoDasPalavras.WinApp
{
    public partial class WordleForm : Form
    {
        private Wordle _wordle;
        private List<MaskedTextBox> _mtbList;
        private int _currentMaskedTextBoxIndex;
        private int _currentAttempt = 1;
        private int _initialIndex1 = 0;
        private int _initialIndex2 = 0;
        public WordleForm()
        {
            InitializeComponent();

            GenerateMtbList();

            SetEvents();

            _wordle = new Wordle();
        }

        private void GenerateMtbList()
        {
            _mtbList = new List<MaskedTextBox>
        {
            mtbFirstInputLineOne,
            mtbSecondInputLineOne,
            mtbThirdInputLineOne,
            mtbFourthInputLineOne,
            mtbFifthInputLineOne,
            mtbFirstInputLineTwo,
            mtbSecondInputLineTwo,
            mtbThirdInputLineTwo,
            mtbFourthInputLineTwo,
            mtbFifthInputLineTwo,
            mtbFirstInputLineThree,
            mtbSecondInputLineThree,
            mtbThirdInputLineThree,
            mtbFourthInputLineThree,
            mtbFifthInputLineThree,
            mtbFirstInputLineFour,
            mtbSecondInputLineFour,
            mtbThirdInputLineFour,
            mtbFourthInputLineFour,
            mtbFifthInputLineFour,
            mtbFirstInputLineFive,
            mtbSecondInputLineFive,
            mtbThirdInputLineFive,
            mtbFourthInputLineFive,
            mtbFifthInputLineFive
        };

        }

        private void SetEvents()
        {
            foreach (Button b in tableLayoutPanel1.Controls)
                b.Click += GetGuess;

            foreach (Button b in tableLayoutPanel2.Controls)
                b.Click += GetGuess;

            foreach (Button b in tableLayoutPanel3.Controls)
            {
                if (b.Tag == "Erase")
                    b.Click += ClearLastInput;

                else if (b.Text == "Enter")
                    b.Click += ValidateGuess;

                else b.Click += GetGuess;
            }
        }

        private void GetGuess(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            _mtbList[_currentMaskedTextBoxIndex].Text = button.Text;

            if (_currentAttempt < _wordle.maxInputsPerAttempt)
            {
                _mtbList[_currentMaskedTextBoxIndex + 1].Focus();
            }
            else
            {
                DisableButtons();
                btnEnter.Focus();
            }

            ++_currentAttempt;
            _currentMaskedTextBoxIndex++;
        }

        private void ClearLastInput(object sender, EventArgs e)
        {
            int j = _mtbList.Count - 1;

            for (int i = j; i >= 0; i--)
            {
                if (!string.IsNullOrWhiteSpace(_mtbList[i].Text))
                {
                    _mtbList[i].Clear();
                    if (i < _currentMaskedTextBoxIndex)
                    {
                        EnableButtons();
                        _currentAttempt--;
                        _currentMaskedTextBoxIndex = i;
                        _mtbList[i].Focus();
                    }
                    break;
                }
            }
        }

        private void ValidateGuess(object sender, EventArgs e)
        {
            _wordle.guess = GetGuessFromMtbList();

            string normalizedGuess = _wordle.guess.RemoveDiacritics();
            string normalizedSecretWord = _wordle.secretWord.RemoveDiacritics();

            if (_wordle.IsValidWord())
            {
                _currentAttempt = 1;
                _currentMaskedTextBoxIndex -= 1;

                for (int i = 0; i < normalizedSecretWord.Length; i++)
                {
                    if (normalizedGuess[i].ToString().ToLower() == normalizedSecretWord[i].ToString().ToLower())
                    {
                        _mtbList[_initialIndex1].BackColor = Color.Green;
                    }
                    else if (normalizedSecretWord.Contains(normalizedGuess[i].ToString().ToLower()))
                    {
                        _mtbList[_initialIndex1].BackColor = Color.Yellow;
                    }
                    else
                    {
                        _mtbList[_initialIndex1].BackColor = Color.Gray;
                    }
                    _initialIndex1++;
                }

                if (normalizedGuess.ToLower() == normalizedSecretWord.ToLower())
                {
                    MessageBox.Show("Parabéns, você acertou a palavra secreta!");
                    ResetGame();
                }
                else if (_currentMaskedTextBoxIndex == _mtbList.Count - 1)
                {
                    MessageBox.Show($"Você errou! :(\n\n A palavra era: {_wordle.secretWord.FirstLetterToUpperCase()}.");
                    ResetGame();
                }
                else
                {
                    _currentMaskedTextBoxIndex++;
                    _mtbList[_currentMaskedTextBoxIndex].Focus();
                }

                EnableButtons();
            }

            else
            {
                MessageBox.Show("Palavra inválida!");

                _initialIndex2 -= 5;

                int NumberOfLetters = _wordle.guess.Length - 1;
                int j = _currentMaskedTextBoxIndex - 1;

                for (int i = NumberOfLetters; i >= 0; i--)
                {
                    _mtbList[j].Clear();
                    _currentAttempt--;
                    _currentMaskedTextBoxIndex--;

                    j--;
                }
            }

        }

        private string GetGuessFromMtbList()
        {
            string guess = "";

            for (int i = _initialIndex2; i < _mtbList.Count; i++)
            {
                guess += _mtbList[i].Text;
            }

            _initialIndex2 += 5;

            return guess;
        }

        private void ResetGame()
        {
            _wordle = new Wordle();
            _currentMaskedTextBoxIndex = 0;
            _currentAttempt = 1;
            _initialIndex1 = 0;
            _initialIndex2 = 0;

            foreach (MaskedTextBox mtb in _mtbList)
            {
                mtb.Clear();
                mtb.BackColor = Color.Gainsboro;
            }

            _mtbList[_currentMaskedTextBoxIndex].Focus();
        }

        private void DisableButtons()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = false;
                }
            }
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = false;
                }
            }
            foreach (Control control in tableLayoutPanel3.Controls)
            {
                if (control is Button buttonControl && buttonControl != btnEnter && buttonControl != btnBackSpace)
                {
                    control.Enabled = false;
                }
            }
        }

        private void EnableButtons()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = true;
                }
            }
            foreach (Control control in tableLayoutPanel2.Controls)
            {
                if (control is Button)
                {
                    control.Enabled = true;
                }
            }
            foreach (Control control in tableLayoutPanel3.Controls)
            {
                if (control is Button buttonControl && buttonControl != btnEnter && buttonControl != btnBackSpace)
                {
                    control.Enabled = true;
                }
            }
        }
    }
}