namespace TicTacToe

module Program =
    [<EntryPoint>]    
    let main args =
        let game = Game(GameControls(GameUtilities(), ComputerPlayer(GameUtilities(), MoveCalculator(GameUtilities()))), GameUtilities())
        game.Start()
        0