using System;
using System.Collections;
using System.Collections.Generic;
using CatSkald.Roguelike.Core.Terrain;

namespace CatSkald.Rogualike.Test.GameProcessor.UnitTests.TestHelpers
{
    public class FakeDungeon : IDungeon
    {
        public Cell this[Cell point]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Cell this[int width, int height]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Height
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Size
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Width
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
