int test1(int d, int e)
{
	int f;
	f = e * d + 30;
	return f;
}

int main()
{
	int a, b, c;

	a = 10;
	b = 20;
	c = test1(a, b);
	return a;
}