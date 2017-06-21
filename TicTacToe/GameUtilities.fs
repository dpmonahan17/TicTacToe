namespace TicTacToe

type IGameUtilities =
    abstract member CheckForTie : grid:Grid -> bool
    abstract member CheckForWin : grid:Grid -> bool
    abstract member BuildBlankBoard : Grid
    abstract member GetPossibleMoves : grid:Grid -> Space List
    abstract member GetSpace : grid:Grid -> position:GridPosition -> Space
    abstract member GetRow : grid:Grid -> index:int -> Space List
    abstract member CheckForXWin : grid:Grid -> bool
    abstract member CheckForOWin : grid:Grid -> bool
    abstract member UpdateGrid : grid:Grid -> selection:Space -> Grid
    abstract member MarkMove : move:Space -> player:Player -> Space

type public GameUtilities() = class

    member x.UpdateGrid (grid:Grid) (selection:Space) =
        (x :> IGameUtilities).UpdateGrid grid selection
    member x.GetPossibleMoves (grid:Grid) : Space List =
        (x :> IGameUtilities).GetPossibleMoves grid
        
    member x.GetSpace (grid:Grid) (position:GridPosition) =
        (x :> IGameUtilities).GetSpace grid position


    member x.GetRow (grid:Grid) (index:int) =
        (x :> IGameUtilities).GetRow grid index

    member x.GetScoreForSpace (space:Space) =
        match space.Marked with
        | X -> 1
        | O -> -1
        | No -> 0

    member x.CheckBoardIsFull (grid:Grid) =
        (grid.grid |> List.filter(fun i -> i.Marked = No)).Length < 1
        
    member x.GetScoreForRow (spaces:Space List) =
        spaces |> List.sumBy(fun i -> x.GetScoreForSpace i)

    member x.CheckForXWin (grid:Grid) =
        (x :> IGameUtilities).CheckForXWin grid

    member x.CheckForOWin (grid:Grid) =
        (x :> IGameUtilities).CheckForOWin grid

    member x.CheckForWin (grid:Grid) =
        (x :> IGameUtilities).CheckForWin grid

    member x.MarkMove move player =
        (x :> IGameUtilities).MarkMove move player

    interface IGameUtilities with
        
        member x.UpdateGrid (grid:Grid) (selection:Space) =
            let position = selection.Position
            let newGrid = 
                grid.grid |> List.map(fun i ->
                    if i.Position = position 
                    then selection
                    else i
                    )
            {grid = newGrid} :Grid

        member x.GetSpace (grid:Grid) (position:GridPosition) =
            grid.grid |> List.find(fun i -> i.Position = position)

        member x.GetRow (grid:Grid) (index:int) =
            match index with
            | 1 -> [x.GetSpace grid (Left,Top); x.GetSpace grid (Left, Center); x.GetSpace grid (Left,Bottom)]
            | 2 -> [x.GetSpace grid (Middle,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Middle,Bottom)]
            | 3 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Right, Center); x.GetSpace grid (Right,Bottom)]
            | 4 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Middle, Top); x.GetSpace grid (Left,Top)]
            | 5 -> [x.GetSpace grid (Right,Center); x.GetSpace grid (Middle, Center); x.GetSpace grid (Left,Center)]
            | 6 -> [x.GetSpace grid (Right,Bottom); x.GetSpace grid (Middle, Bottom); x.GetSpace grid (Left,Bottom)]
            | 7 -> [x.GetSpace grid (Right,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Left,Bottom)]
            | 8 -> [x.GetSpace grid (Left,Top); x.GetSpace grid (Middle, Center); x.GetSpace grid (Right,Bottom)]
            
        member x.CheckForXWin (grid:Grid) =
            let rows : List<List<Space>> = 
                [1;2;3;4;5;6;7;8] |> List.map(fun i -> x.GetRow grid i)
            let result = 
                rows |> List.map(fun i -> x.GetScoreForRow i)
            result |> List.contains(3)

        member x.CheckForOWin (grid:Grid) =
            let rows : List<List<Space>> = 
                [1;2;3;4;5;6;7;8] |> List.map(fun i -> x.GetRow grid i)
            let result = 
                rows |> List.map(fun i -> x.GetScoreForRow i)
            result |> List.contains(-3)

        member x.CheckForWin (grid:Grid) =
            x.CheckForXWin grid || x.CheckForOWin grid

        member x.CheckForTie (grid:Grid) =
            x.CheckBoardIsFull grid && not (x.CheckForWin grid)

        member x.GetPossibleMoves (grid:Grid) = 
            let newGrid :List<Space> = 
                grid.grid |> List.filter(fun i -> i.Marked = No)
            newGrid
        member x.BuildBlankBoard =
            let spaces = [
                {Position = (Left, Top); Marked = No};
                {Position = (Left, Center); Marked = No};
                {Position = (Left, Bottom); Marked = No};
                {Position = (Middle, Top); Marked = No};
                {Position = (Middle, Center); Marked = No};
                {Position = (Middle, Bottom); Marked = No};
                {Position = (Right, Top); Marked = No};
                {Position = (Right, Center); Marked = No};
                {Position = (Right, Bottom); Marked = No};
            ]
            {grid = spaces} :Grid

        member x.MarkMove move player =
            {Position = move.Position; Marked = player.Mark}
        
end