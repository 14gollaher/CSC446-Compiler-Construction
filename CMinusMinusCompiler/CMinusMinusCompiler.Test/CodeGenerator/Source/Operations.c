int a, b;

int addition()
{
	return a + b;
}

int subtraction()
{
	return a - b;
}

int multiplication()
{
	return a * b;
}

int logicalAnd()
{
	return a && b;
}

int logicalOr()
{
	return a || b;
}

int logicalNot()
{
	/*a = !a;*/
	return !a;
}

int negation()
{
	return -a;
}

int division()
{
	return a / b;
}

int modulo()
{
	return a % b;
}

int main()
{
	int result;
	cout << "Enter value for a: ";
	cin >> a;
	cout << "Enter value for b: ";
	cin >> b;
	a = a + 5;

	cout << "a is: " << a << endl
		 << "b is: " << b << endl;
	result = addition();
	cout << "a + b = " << result << endl;
	
	result = subtraction();
	cout << "a - b = " << result << endl;

	result = multiplication();
	cout << "a * b = " << result << endl;

	result = logicalAnd();
	cout << "a && b = " << result << endl;

	result = logicalOr();
	cout << "a || b = " << result << endl;

	result = logicalNot();
	cout << "!a = " << result << endl;
	
	result = negation();
	cout << "-a = " << result << endl;

	result = division();
	cout << "a / b = " << result << endl;

	result = modulo();
	cout << "a % b = " << result << endl;

	return -1;
}


