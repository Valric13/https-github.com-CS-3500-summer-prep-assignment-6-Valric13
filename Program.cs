using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assign6Project
{
    /// <summary>
    /// A class which evaluates infix expressions.
    /// </summary>
    /// <author>
    /// Chase Averett
    /// </author>
    public class InfixCalculator
    {
        static void Main(string[] args)
        {
       
        }

        /// <summary>
        /// A method which evaluates an infix expression given as a String.
        /// </summary>
        /// <param name="infixExpression"></param>
        /// <returns>
        /// The result of an infix expression, by returning a postfix expression.
        /// </returns>
        /// <exception cref="SystemException">Thrown if the number of opening
        /// and closing parathesis do not match.</exception>
        public static int Evaluate (String infixExpression)
        {
            Stack<string> operatorStack = new Stack<string>();
            String[] splitted = infixExpression.Trim().Split(' ');
            StringBuilder infixString = new StringBuilder(); // StringBuilder to compose a String which is a postfix expression.
            String variable;
            int openingParenthesisCount = 0;

            for (int i = 0; i < splitted.Length; i++)
            {
                variable = splitted[i];  // Variable of the infix expression derived from the splitted array at index "i".

                // A switch is used to evaluate the operator and will push it onto the operatorStack.
                // It will either push or pop based on the rules of the order of operations. When a variable
                // is popped, it is appended to a postfix expression String. If the infix expression contains
                // an undefined operator, the ordering is invalid, or the operator and operand ratio is invalid
                // a SystemException is thrown.

                switch (variable)
                {
                    case "(":
                        operatorStack.Push(variable);
                        openingParenthesisCount++;
                        break;
                    case "^":
                        operatorStack.Push(variable);
                        break;
                    case ")":
                        if (openingParenthesisCount < 1)  // throws a SystemException if there isn't an opening parenthesis in the infix expression.
                            throw new Exception("Invalid infix expression entered. Missing opening parenthesis");
                        while (!(operatorStack.Count == 0) && (!(operatorStack.Peek().Equals("("))))
                            infixString.Append(operatorStack.Pop() + ' ');
                        if (!(operatorStack.Count == 0))
                        {
                            operatorStack.Pop();
                            openingParenthesisCount--;
                        }
                        break;
                    case "*": case "/": case "%":
                        while (!(operatorStack.Count == 0) && !(operatorStack.Peek().Equals("(")) &&
                                (operatorStack.Peek().Equals("*") || operatorStack.Peek().Equals("/") ||
                                operatorStack.Peek().Equals("%")))
                            infixString.Append(operatorStack.Pop() + ' ');
                        operatorStack.Push(variable);
                        break;
                    case "+": case "-":
                        while (!(operatorStack.Count == 0) && (!(operatorStack.Peek().Equals("(")) &&
                            (operatorStack.Peek().Equals("*")) || operatorStack.Peek().Equals("/") ||
                            operatorStack.Peek().Equals("%") || operatorStack.Peek().Equals("+") ||
                            operatorStack.Peek().Equals("-")))
                            infixString.Append(operatorStack.Pop() + ' ');
                        operatorStack.Push(variable);
                        break;
                    default:
                        infixString.Append(variable + ' ');
                        break;
                }
            }

            // Will finish evaluating any remaining operators in the operatorStack after the switch
            // has evaluated all of the tokens in the infix expression String. The result is a String
            // which is a postfix expression.

            while (!(operatorStack.Count == 0))
                infixString.Append(operatorStack.Pop() + ' ');

            // Returns the result of the infix expression as a postfix expression String which is then
            // used as the parameter upon which the PostfixCalculator class is called to get the final result.

            return evaluatePostfix(infixString.ToString());
        }

        /// <summary>
        /// A method which evaluates an infix expression given as a String.
        /// </summary>
        /// <param name="postfixExpression"></param>
        /// <returns>
        /// Ultimately returns the result of the infix expression by evaluating
        /// the postfix expression parameter
        /// </returns>
        /// <exception cref="SystemException">Thrown if the psotfix expression is
        /// invalid, or division by zero occurs.</exception>
        
        public static int evaluatePostfix (String postfixExpression)
        {
            Stack<int> operandStack = new Stack<int>();
            int firstOperand;
            int secondOperand;
            int operandCount = 0;  // Variable to track the number of operands.
            int operatorCount = 0;  // Variable to track the number of operators.
            int operand;
            bool isNumeric; // Determines if the string is an integer.
            String variable;
            String[] splitted = postfixExpression.Trim().Split(' ');  // Array which contains the postfix expression split on the whitespace.

            for (int i = 0; i < splitted.Length; i++)
            {
                variable = splitted[i];
                isNumeric = int.TryParse(variable, out operand);

                // Pushes operand onto the Stack if the String is an operand.
                if (isNumeric)
                    {
                    operandStack.Push(operand);
                    operandCount++;
                }
                // Throws a SystemException if the postfix expression attempts to divide by 0.
                else if (variable.Contains("/") && operandStack.Peek() == 0)
                    throw new Exception("Dividing by zero is undefined");

                // Throws a SystemException in the event of an invalid postfix expression
                // in the event that there are not enough operands to carry out the operation.
                else if (operandStack.Count < 2)
                    throw new Exception("Invalid expression entered.");

                // A switch is used to evaluate the operator and carries out the operation based
                // on which type of operator the String contains. The result is then pushed to the 
                // operand Stack. If the postfix expression contains an undefined operator, a 
                // SystemException is thrown.
                else
                    switch (variable)
                    {
                        case "+":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push(firstOperand + secondOperand);
                            operatorCount++;
                            break;
                        case "-":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push(firstOperand - secondOperand);
                            operatorCount++;
                            break;
                        case "*":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push(firstOperand * secondOperand);
                            operatorCount++;
                            break;
                        case "/":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push(firstOperand / secondOperand);
                            operatorCount++;
                            break;
                        case "^":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push((int)Math.Pow(firstOperand, secondOperand));
                            operatorCount++;
                            break;
                        case "%":
                            secondOperand = operandStack.Pop();
                            firstOperand = operandStack.Pop();
                            operandStack.Push(firstOperand % secondOperand);
                            operatorCount++;
                            break;
                        default:
                            throw new Exception("Invalid expression entered");
                    }
            }

            // A SystemException is thrown in the event that the postfix expression is invalid
            // due to an insufficient number of operators to operand count.
            if (operatorCount != operandCount - 1)
                throw new Exception("Invalid expression entered");

            // Final result of the postfix expression is returned.
            else
                return operandStack.Pop();
        }
    }
}
