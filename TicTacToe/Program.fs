namespace TicTacToe

module Program =
    [<EntryPoint>]    
    let main args =
        let game = Game(GameControls(GameUtilities(), ComputerPlayer(GameUtilities(), Algorithm(GameUtilities()))), GameUtilities())
        game.Start()
        0