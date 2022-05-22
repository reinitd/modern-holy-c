#include <string.h>
#include <stdlib.h>

#ifndef MHCSTRINGS_H
#define MHCSTRINGS_H

inline void assign_heap_str(char *arr, char *newstr)
{
    int len = strlen(newstr);
    for(int i = 0; i < len; i++)
    {
        arr[i] = newstr[i];
    }
}

#endif