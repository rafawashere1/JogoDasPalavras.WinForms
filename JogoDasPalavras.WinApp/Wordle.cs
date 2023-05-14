namespace JogoDasPalavras.WinApp
{
    public class Wordle
    {
        public readonly string secretWord;
        public string guess;
        public readonly int maxInputsPerAttempt;

        public Wordle()
        {
            secretWord = GetSecretWord();
            maxInputsPerAttempt = 5;
        }

        public bool IsValidWord()
        {
            if (guess.Length == 5)
                return true;

            else
                return false;
        }
        private string GetSecretWord()
        {
            string[] words = new string[]
{
    "ácido"
};

            int randomIndex = new Random().Next(words.Length);

            return words[randomIndex];
        }
    }
}
