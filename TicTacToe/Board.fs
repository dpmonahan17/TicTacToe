namespace TicTacToe


module Board =

    let printBoardGrid =
        printf "Board Index - Press Number for grid to place marker"
        printf " 1 | 2 | 3 \n"
        printf "===========\n"
        printf " 4 | 5 | 6 \n"
        printf "===========\n"
        printf " 7 | 8 | 9 \n"

    let printBoard (grid:Grid) =
        printf " %c | %c | %c \n" (grid.GetSpace 1).Mark (grid.GetSpace 2).Mark (grid.GetSpace 3).Mark
        printf "===========\n"
        printf " %c | %c | %c \n" (grid.GetSpace 4).Mark (grid.GetSpace 5).Mark (grid.GetSpace 6).Mark
        printf "===========\n"
        printf " %c | %c | %c \n" (grid.GetSpace 7).Mark (grid.GetSpace 8).Mark (grid.GetSpace 9).Mark

    let printWinner player =
        printf "Player %s has won" player

    let printNoWinner =
        printf "Game has ended in stalement"

    let printInvalidCharacter player =
        printf "Last entry was invalid, player: %s please try again." player
