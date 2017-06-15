namespace TicTacToeTests

    module GameTests =

        open NUnit.Framework
        open FsUnit

        open TicTacToe
    
        [<Test>]
        let ``Game should have a player 1`` () =
            let game = Game()
            game.PlayerOne :? Player |> should equal true

        [<Test>]
        let ``Game should have a player 2`` () =
            let game = Game()
            game.PlayerTwo :? Player |> should equal true

        [<Test>]
        let ``Game should have a board`` () =
            let game = Game()
            game.GetGrid :? Grid |> should equal true

        // [<Test>]
        // let ``State should have three types`` () =
            