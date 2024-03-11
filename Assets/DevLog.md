Until now I have done a pretty simple, brute force apporoach. The Grid is stored in a BlobAssetReference on the Grid Entity. The Grid is rendered by Spawning Quads which are being enabled or disabled based on the life. This seems to be the number one performance culprit for now. So this is what I am trying to speed up first.

I have changed the rendering to changing the material color of the quard, instead of disabling / enabling it.

I also rewrote the Job that does the Simulation to no longer use an EntityCommandBuffer. I also put the chaning of the Material into the same Job!

Using custom Profiler Markers, I found out that part of my SimulateLife System that is not jobified was slow!

So I have basically now done a lot of work. I got rid of All BlobAssets and purely rely on Entities for my data. This was a big speedup!

For Rendering I no longer render a Mesh Per Cell, but instead only have one mesh wit a texture that gets updated each frame.

I used the profiler to find a lot of small and big gains. Working with managed data introduced some Lag Spikes due to Garbage Collection, which I got rid of by using other APIs on the Texture2D Class to directly write into the Color Data Array, which fortunately is a NativeArray which can be accessed in Jobs. Right Now My Code seems to be at least 10x faster then it was yesterday, but also, the simulation is acting weird. I might have broken it.

I added some tests for my simulation code, to figure out why my simulation was acting weird. I found a small bug in my code when determining the neighbors of a cell which lead to weird results. Now the simulation seems to be great. Also the performance is pretty dope, considering that I haven't really put much effort into optimizing data structures etc.

Last I made some Tweaks to the Ui. Even though I still had some days left, I felt like that I had gotten the maximum DOTS learning out of the project. The rest would be trying to optimize the algorithms and data structures, which I might get back to in the future but for now I feel like moving on to other projects after spening at least 20 hours on this.