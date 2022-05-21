#include <stdlib.h>
#include <string.h>
#include <stdio.h>

void AssignToPointer(char **arr, char *newstr)
{
    int len = strlen(newstr);
    for(int i = 0; i < len; i++)
    {
        *arr[i] = newstr[i];
    }
}

int main()
{
    char x[12] = "Hello World!";
    char *heapstr = malloc(12 * sizeof(char));
    AssignToPointer(&heapstr, "Hi!");

    printf("%s", heapstr);
}