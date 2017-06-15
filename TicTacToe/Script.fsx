#load "TicTacToe.fs"
open TicTacToe

// Define your library scripting code here

let adderWithPluggableLogger logger x y = 
    logger "x" x
    logger "y" y
    let result = x + y
    logger "sum"  result 
    result 

// create a logging function that writes to the console
let consoleLogger argName argValue = 
    printf "%s=%A" argName argValue 

//create an adder with the console logger partially applied
let addWithConsoleLogger = adderWithPluggableLogger consoleLogger

addWithConsoleLogger  1 2 
addWithConsoleLogger  42 99
addWithConsoleLogger  10 11


let (>>) f g x = g ( f(x) )

let add n x = x + n
let times n x = x * n
let add1Times2 = add 1 >> times 2
