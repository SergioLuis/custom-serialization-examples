# Serialization examples

In this repository you can find the serialization examples I talk about in [my personal blog](http://blog.sergioluis.es/hablemos-de-serializacion).

The results may vary between computers, but the custom serialization mechanism is expected to achieve the best performance everywhere. It has no memory overhead, as it only writes primitive types into a stream.

## Benchmark
Asus X550CA laptop
* Intel Core i5-3337U @ 1.80GHz
* 8 GB of RAM

> Created 100000 SeaMonster objects in 1422 ms
> Serialized 100000 SeaMonster objects the C# way in 7718 ms.
> The stream has 82183122 bytes written.
> Deserialized 100000 SeaMonster objects the C# way in 263516 ms.
> Serialized 100000 SeaMonster objects the custom way in 328 ms.
> The stream has 31914195 bytes written.
> Deserialized 100000 SeaMonster objects the custom way in 735 ms. 
> 