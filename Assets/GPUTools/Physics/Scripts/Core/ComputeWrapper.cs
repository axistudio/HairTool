using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GPUTools.Physics.Scripts.Core
{
    public class ComputeWrapper
    {
        private readonly ComputeShader shader;
        private readonly Dictionary<string, ComputeBuffer> buffers; 

        public ComputeWrapper(ComputeShader shader)
        {
            this.shader = shader;
            buffers = new Dictionary<string, ComputeBuffer>();
        }

        public void AddBuffer(string name, ComputeBuffer buffer)
        {
            buffers.Add(name, buffer);
        }

        public void CreateBuffer<T>(string name, T[] array, int stride)
        {
            Assert.IsFalse(buffers.ContainsKey(name), string.Format("ComputeBuffer {0} already created", name));

            var buffer = new ComputeBuffer(array.Length, stride);
            buffer.SetData(array);

            buffers.Add(name, buffer);
        }

        public void SetBufferData<T>(string name, T[] array)
        {
            Assert.IsTrue(buffers.ContainsKey(name), string.Format("Can't find buffer {0}", name));
            buffers[name].SetData(array);
        }

        public void GetBufferData<T>(string name, T[] array)
        {
            Assert.IsTrue(buffers.ContainsKey(name), string.Format("Can't find buffer {0}", name));
            buffers[name].GetData(array);
        }

        public ComputeBuffer GetBuffer(string name)
        {
            Assert.IsTrue(buffers.ContainsKey(name), string.Format("Can't find buffer {0}", name));
            return buffers[name];
        }

        public void DispatchKernel(int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
        {
            AssignBuffersToKernel(kernelIndex);
            shader.Dispatch(kernelIndex,threadGroupsX, threadGroupsY, threadGroupsZ);
        }

        private void AssignBuffersToKernel(int kernelIndex)
        {
            foreach (var pair in buffers)
            {
                shader.SetBuffer(kernelIndex, pair.Key, pair.Value);
            }
        }

        public void Dispose()
        {
            foreach (var pair in buffers)
            {
                pair.Value.Dispose();
            }
        }
    }
}
