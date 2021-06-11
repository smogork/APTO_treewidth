#! /usr/bin/python

import sys
import os

if __name__ == '__main__':

    if len(sys.argv) < 5:
        exit(1)

    v = int(sys.argv[1])
    e = int(sys.argv[2])
    count = int(sys.argv[3])
    output_folder = sys.argv[4]

    for i in range(count):
        
        print("graph", i)

        os.system(f'./Randomizer/bin/Debug/net5.0/Randomizer {v} {e} > {output_folder}/g_{v}_{e}_{i}.gr')

            