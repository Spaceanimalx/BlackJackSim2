namespace bjs
{
    public class GameStateEventArgs : EventArgs
    {
        private bool playerHasNatural21 { get; set; }
        public int idx;
        public bool playerIsBust;
        public GameStateEventArgs(int targetindex)
        {
            idx = targetindex;
        }
        public GameStateEventArgs(bool busted)
        {
            playerIsBust = busted;
        }
    }
}
