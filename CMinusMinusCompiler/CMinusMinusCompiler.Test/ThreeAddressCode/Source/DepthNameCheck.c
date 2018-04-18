int a;
int foo()
{
	int a;
	return a * a + a;
}

int b;
int main()
{
	int a;

	a = a + a * a;
	a = foo(a, b);
	return a;
}