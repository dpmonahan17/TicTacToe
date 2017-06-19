namespace TicTacToeTests

    module GameModuleTests =

        open NUnit.Framework
        open FsUnit
        open Foq

        open TicTacToe
    
        [<Test>]
        let ``Game should print game results only if game is completed`` () =
            let mock = 
                Mock<IGameControls>()
                    .Setup(fun i -> <@ i.PrintGameResults( any()* any()) @>)
                    .Returns(CallsAttribute)
                
            let board = 
               Completed {player = {name = "John Doe"; mark = X}; result = Win }
            board |> Game.printResults
            mock.Calls() |> should equal 1
        