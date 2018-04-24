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

int division()
{
	return a / b;
}

int modulo()
{
	return a % b;
}

int logicalAnd()
{
	return a && b;
}

int logicalOr()
{
	return a || b;
}

int main()
{
	int d, e, result;

	cout << "Enter value for a: ";
	cin >> a;
	cout << "Enter value for b: ";
	cin >> b;

	result = addition();
	cout << "a + b = " << result << endl;
	
	result = subtraction();
	cout << "a - b = " << result << endl;

	result = multiplication();
	cout << "a * b = " << result << endl;

	result = division();
	cout << "a / b = " << result << endl;

	/*result = modulo();*/
	cout << "a % b = " << result << endl;

	result = logicalAnd();
	cout << "a && b = " << result << endl;

	result = logicalOr();
	cout << "a || b = " << result;

	return -1;
}