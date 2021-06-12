#! /usr/bin/python

import argparse
import subprocess
import threading
import time
import sys

def pass_stdin(p):
    while True:
        data = input()
        p.stdin.write(data)

if __name__ == '__main__':
    parser = argparse.ArgumentParser(description='Count time of running command.')
    parser.add_argument('command', metavar='cmd', type=str, nargs=1)
    args = parser.parse_args()

    cmd = args.command[0].split()

    start = time.time()
    p = subprocess.Popen(cmd, stdout=subprocess.PIPE, stdin=subprocess.PIPE)

    input_reader = threading.Thread(target=pass_stdin, args=(p,), daemon=True)
    input_reader.start()

    # Grab stdout line by line as it becomes available.  This will loop until 
    # p terminates.
    while p.poll() is None:
        l = p.stdout.readline() # This blocks until it receives a newline.
        if l == b'':
            continue
        print(l.decode(), end='')
    # When the subprocess terminates there might be unconsumed output 
    # that still needs to be processed.
    last_line = p.stdout.read()
    if last_line != b'':
        print(last_line.decode(), end='')
    end = time.time()

    print(end - start, file=sys.stderr)

