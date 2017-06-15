namespace TicTacToe

module Scorer = 

    let getValue x =
        match x with
        | 'x' -> 1
        | 'o' -> -1
        | _ -> 0

    let getScoreForSpace (space:Space) =
        getValue space.Mark

    let getScoreForRow (spaces:List<Space>) =
        spaces |> List.sumBy(fun i -> getScoreForSpace i)

    let checkForWinningRow (grid:Grid) =
        [1..8] |> List.map(fun i ->
            let score = 
                getScoreForRow (grid.GetRow i)
            score = 3 || score = -3
        )

    let checkForWinner (grid:Grid) =
        checkForWinningRow grid |> 
        List.exists(id)
     
    let getFirstSpaceInRow (grid:Grid) (index:int) =
        grid.GetRow index |> List.item 0 

    let winningRowIndex (grid:Grid)=  
        checkForWinningRow grid |>
            List.findIndex(id)

    let getWinningMarker (grid:Grid) =
        let adjustedRowIndex = 
            ((winningRowIndex grid) + 1)
        let space = 
            getFirstSpaceInRow grid adjustedRowIndex
        space.Mark
        