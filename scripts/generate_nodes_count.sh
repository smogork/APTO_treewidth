cd ..
for (( k = 20; k < 300; k = k + 20 ));
do
    n = $((i * 20))
    b = $((n / 5))
    dotnet /home/oskar/RiderProjects/APTO_treewidth/Randomizer/bin/Debug/net5.0/Randomizer.dll -t 5 $n $b
done