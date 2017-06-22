namespace TicTacToe

type IGameControls = 
    abstract member PrintGameResults : grid:Grid -> results:GameResult -> player:Player -> unit
    abstract member GetTurn : grid:Grid -> player:Player -> Grid
    abstract member ChooseMark : Mark
    abstract member GetPlayerData : Player

type public GameControls(gameUtils : IGameUtilities, pcPlayer : IComputerPlayer) = class

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
        System.Console.Clear()
        printf "Board Index - Press Number for grid to place marker\n"
        printf " 1 | 2 | 3 \n"
        printf "===========\n"
        printf " 4 | 5 | 6 \n"
        printf "===========\n"
        printf " 7 | 8 | 9 \n\n"
        printf " %s | %s | %s \n" (getMark grid 1) (getMark grid 2) (getMark grid 3)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 4) (getMark grid 5) (getMark grid 6)
        printf "===========\n"
        printf " %s | %s | %s \n" (getMark grid 7) (getMark grid 8) (getMark grid 9)

    let isInValidMove (grid:Grid) (position:GridPosition) =
        let index = (grid.grid |> List.filter(fun i -> i.Marked = No) |> List.tryFind(fun i -> i.Position = position))
        match index with
        | None -> true
        | _ -> false

    let rec GetPlayerInput (grid:Grid) player =
        printf "Please press number to select where to play\n"
        let selection : string = System.Console.ReadLine()
        match selection with 
        | "1" | "2" | "3" | "4" | "5" | "6" | "7" | "8" | "9" ->
            let position = selection |> int |> getPositionPerIndex
            if isInValidMove grid position then
                printf "Please select empty space\n"
                GetPlayerInput grid player
            else
                let space = {
                    Position = position
                    Marked = player.Mark
                }
                gameUtils.UpdateGrid grid space
        | _ ->
            printf "Please select number 1-9\n"
            GetPlayerInput grid player

    member x.PrintGameResults (grid:Grid) (results:GameResult) (player:Player) =
        (x :> IGameControls).PrintGameResults grid results player
    member x.GetTurn (grid:Grid) (player:Player) =
        (x :> IGameControls).GetTurn grid player
    member x.ChooseMark =
        (x :> IGameControls).ChooseMark

    member x.GetPlayerData : Player =
        (x :> IGameControls).GetPlayerData
    
    interface IGameControls with
    
        member x.PrintGameResults (grid:Grid) (results:GameResult) (player:Player) =
            PrintCurrentGrid grid
            match results with
            | Win -> printf "Player %s has won!\n" player.Name
            | Tie -> printf "Game has ended in a tie\n"
            System.Console.ReadKey()
            printf "Ctrl+c to quit at any time....\nStarting Over\n"

        member x.GetTurn grid player =
            match player.Name with
            | "PC Player" -> 
                gameUtils.UpdateGrid grid (pcPlayer.GetNextMove grid player)
            | _ ->
                PrintCurrentGrid grid
                GetPlayerInput grid player

        member x.ChooseMark : Mark=
            printf "Please pick your mark: 1 for X or 2 for O\n"
            let mark : string = System.Console.ReadLine()
            match mark with
            | "x" | "X" | "1" -> X
            | "o" | "O" | "2" -> O
            | _ -> 
                printf "invalid character chosen\n"
                x.ChooseMark

        member x.GetPlayerData : Player=
            printf "Please enter your name: "
            let name = System.Console.ReadLine()
            let mark = x.ChooseMark
            {Name = name; Mark = mark}

end