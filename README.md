# Canonical form for equation
This application turns equations into a simplified canonical form
Examples:

"x^2 + 3.5xy + y = y^2 - xy + y" into "x^2 - y^2 + 4.5xy = 0" (or a equivalent equation)
"x = 1" into "x - 1 = 0"
"x^2 - (y - x^2) = 0" into "2x^2 - y = 0"
"x * (2 + 3x + 4y^3) = 0" into "4xy^3 + 3x^2 + 2x = 0"
To do this, it convert input equation into postfix form, solving precedence and finally simplifies and reassembles it as a canonical equation.
