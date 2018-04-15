int main()
{
	int y;
	y = y(); /*SHOULD THIS BE ALLOWED?*/
	return -1;
}

int foo()
{
	int x;
	x = main/*WILL THIS BE OMMITED?*/();
	int y = 3;
	return -2;
}
/*This will be omitted!!!*/
