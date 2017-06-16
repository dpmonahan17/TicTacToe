namespace TicTacToeTests

    module TurnStateTests =

        open NUnit.Framework
        open FsUnit
        
        open TicTacToe

        let playerOne = Player("Name", 'x')
        let playerTwo = Player("Game", 'o')    

        [<Test>]
        let ``Should contain two players`` () =
            1 |> should equal 1
