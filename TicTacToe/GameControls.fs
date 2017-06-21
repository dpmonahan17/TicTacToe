namespace TicTacToe

type IGameControls = 
    abstract member PrintGameResults : results:GameResult -> player:Player -> unit
    abstract member GetTurn : grid:Grid -> player:Player -> Grid
    abstract member ChooseMark : Mark
    abstract member GetPlayerData : Player

type public GameControls(gameUtils : IGameUtilities) = class

    let gameUtils = gameUtils

    let markToString (mark:Mark) =
        match mark with
        | X -> "x"
        | O -> "o"
        | No -> " "

    let getPositionPerIndex (index:int) =
        match index with
        | 1 -> (Left,Top)
        | 2 -> (Middle,Top)
        | 3 -> (Right,Top)
        | 4 -> (Left,Center)
        | 5 -> (Middle,Center)
        | 6 -> (Right,Center)
        | 7 -> (Left,Bottom)
        | 8 -> (Middle,Bottom)
        | 9 -> (Right,Bottom)

    let getMark (grid:Grid) (index:int) =
        (gameUtils.GetSpace grid (getPositionPerIndex index)).Marked |> markToString
        
    let PrintCurrentGrid grid =
        printf "Board Index - Press Number for grid to place marker"
        printf " 1 | 2 | 3 \n"
        printf "===========\n"
        printf " 4 | 5 | 6 \n"
        printf "===========\n"
        printf " 7 | 8 | 9 \n\n\n"
        printf " %s | %s | %s \n" (getMark grid 1) (getMark grid 2) (getMark grid 3)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 4) (getMark grid 5) (getMark grid 6)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 7) (getMark grid 8) (getMark grid 9)

    let PrintPlayerOptions grid =
        printf "options"

    let GetPlayerInput (grid:Grid) player =
        printf "Please press number to select where to play"
        let selection : string = System.Console.ReadLine()
        let position = selection |> int |> getPositionPerIndex
        let space = {
            Position = position
            Marked = player.Mark
        }
        gameUtils.UpdateGrid grid space

    member x.ChooseMark =
        (x :> IGameControls).ChooseMark
    
    interface IGameControls with
    
        member x.PrintGameResults (results:GameResult) (player:Player) =
            match results with
            | Win -> printf "Player %s has won!" player.Name
            | Tie -> printf "Game has ended in a tie"

        member x.GetTurn grid player=
            PrintCurrentGrid grid
            PrintPlayerOptions grid
            GetPlayerInput grid player

        member x.ChooseMark : Mark=
            printf "Please pick your mark: 1 for X or 2 for O"
            let mark : string = System.Console.ReadLine()
            match mark with
            | "x" | "X" | "1" -> X
            | "o" | "O" | "2" -> O
            | _ -> 
                printf "invalid character chosen"
                x.ChooseMark

        member x.GetPlayerData : Player=
            printf "Please enter your name: "
            let name = System.Console.ReadLine()
            let mark = x.ChooseMark
            {Name = name; Mark = mark}

end