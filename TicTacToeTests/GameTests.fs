namespace TicTacToeTests

    module GameModuleTests =

        open NUnit.Framework
        open FsUnit
        open Foq

        open TicTacToe
    
        type fakeGameControls() = class
            
            let mutable callCount = ref 0

            member x.CallCount:int ref = callCount

            interface IGameControls with
                            
                member x.PrintGameResults (results:GameResult) (player:Player) =
                    incr callCount
                    
                member x.GetTurn grid player=
                    grid

        end
        
        [<Test>]
        let ``Game should print game results only if game is completed`` () =
            let fake = fakeGameControls()
            let utilities = GameUtilities()
            let game = Game(fake, utilities)                    
            let board = 
                Completed {player = {name = "John Doe"; mark = X}; result = Win }
            
            board |> game.printResults 

            fake.CallCount.Value |> should equal 1
        