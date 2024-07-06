using System.Collections;
using System.Collections.Generic;
//Script que declara los enums a utilizar en el DeckCreator
public enum TokenTypes{//Tipos de tokens
     number,// 1 2 3 4 5
     literal,// "Un string"

     //Identificadores
          identifier,// for int
          cardAssignment,//Name Type Faction
          blockDeclaration,//card effect
     //

     //Simples
          punctuator,// : ; , . " '
          limitator,// ( ) [ ] { }
          //Operators
               booleanOp,// == != < > <= >= && ||
               arithemticOp,// + - * / ^ += -= *= /= ^=
               concatOp,// @ @@
               asignationOp,//=
               unaryOp,// ++ --
               lambdaOp,// =>
     //

     //Otros
          end,// $
          unexpected// ?
     //
};
