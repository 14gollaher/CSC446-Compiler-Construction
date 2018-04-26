int calculateSum(int x, int y)
{
	int a, b;
	a = x;
	b = y;
	return a + b;
}

int main()
{
	int x, y;
	x = -1;
	y = 5;
	x = calculateSum(x, y);
	cout << "The sum is: " << x;
	return -1;
}

