int main()
{
	int y;
	y = y(/*WILL THIS BE OMMITED?*/ y ,  /*WILL THIS BE OMMITED?*/ y /*WILL THIS BE OMMITED?*/ , /*WILL THIS BE OMMITED?*/ 2 /*WILL THIS BE OMMITED?*/ )/*WILL THIS BE OMMITED?*/; /*SHOULD THIS BE ALLOWED?*/
	return -1;
}

int foo()
{
	int x;
	x = main/*WILL THIS BE OMMITED?*/();
	x = x(x, 2.3); /*SHOULD THIS BE ALLOWED?*/ 
	return -2;
}
/*This will be omitted!!!*/
