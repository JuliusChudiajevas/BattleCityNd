using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleCity
{
    class CollisionResponse
    {
        public Try result { get; }
        public TileInformation movingEntity { get; }
        public TileInformation collidedWith { get; }

        public CollisionResponse()
        {
            this.result = Try.Success;
            this.movingEntity = null;
            this.collidedWith = null;
        }

        public CollisionResponse(TileInformation movingEntity, TileInformation collidedWith)
        {
            this.result = Try.Failure;
            this.movingEntity = movingEntity;
            this.collidedWith = collidedWith;
        }
    }
    class TryTileInformation
    {
        public Try result { get; }
        public TileInformation info { get; }
        public TryTileInformation() { this.result = Try.Failure; this.info = null; }
        public TryTileInformation(TileInformation info) { this.result = Try.Success; this.info = info; }
    }
    class MoveRequest
    {
        public Rect oldPosition { get; set; }
        public Rect newPosition { get; set; }
        public MoveRequest(Rect oldPosition, Rect newPosition)
        {
            this.oldPosition = oldPosition;
            this.newPosition = newPosition;
        }
    }
    class MoveResponse
    {
        Try response { get; set; }
        int movedEntity { get; set; }
        int collidedWith { get; set; }

    }
    class TileInformation
    {
        public int entityId { get; set; }
        public Rect position { get; set; }
        public TileInformation(int entityId, Rect position)
        {
            this.entityId = entityId;
            this.position = position;
        }
    }
    class WorldMap
    {
        private int mapWidth = 800;
        public int getWidth => this.mapWidth;
        private int mapHeight = 800;
        public int getHeight => this.mapHeight;
        private TileInformation[,] map;

        public WorldMap()
        {
            initializeMap();
        }

        private void initializeMap()
        {
            map = new TileInformation[mapWidth, mapHeight];

            for (int i = 0; i < mapWidth; i++)
            {
                for (int j = 0; j < mapHeight; j++)
                {
                    map[i, j] = null;
                }
            }
        }

        public TryTileInformation tryGetInfoFromPosition(Rect position)
        {
            var info = map[position.x, position.y];
            if (info == null) return new TryTileInformation();

            return new TryTileInformation(info);
        }

        private CollisionResponse checkCollision(Rect oldPosition, Rect newPosition)
        {
            var tryTileInfo = tryGetInfoFromPosition(oldPosition);
            if (tryTileInfo.result == Try.Failure)
            {
                throw new Exception();
            }

            var currentInfo = tryTileInfo.info;

            for (int i = newPosition.left; i < newPosition.right; i++)
            {
                for (int j = newPosition.top; j < newPosition.bottom; j++)
                {
                    var tile = map[i, j];
                    if (tile != null)
                    {
                        if (tile.entityId != currentInfo.entityId)
                        {
                            return new CollisionResponse(currentInfo, tile);
                        }
                    }
                }
            }

            return new CollisionResponse();
        }
        public CollisionResponse tryMoveInformation(MoveRequest request)
        {
            var oldPosition = request.oldPosition;
            var newPosition = request.newPosition;

            var oldTileInfo = map[oldPosition.x, oldPosition.y];

            if (oldTileInfo == null) throw new Exception();
            if (!isCorrectRemoval(oldTileInfo)) throw new Exception();

            var collisionResponse = checkCollision(oldPosition, newPosition);

            if (collisionResponse.result == Try.Success)
            {
                var removeResonse = tryRemoveInformation(oldTileInfo);
                if (removeResonse == Try.Failure)
                {
                    throw new Exception();
                }

                var putResponse = tryPutInformation(new TileInformation(oldTileInfo.entityId, newPosition));
                if (putResponse == Try.Failure)
                {
                    throw new Exception();
                }
            }

            return collisionResponse;
        }

        public Try tryPutInformation(TileInformation info)
        {
            var pos = info.position;
            for (int i = pos.left; i < pos.right; i++)
            {
                for (int j = pos.top; j < pos.bottom; j++)
                {
                    var tile = map[i, j];
                    if (tile != null)
                    {
                        // throw new Exception();
                        return Try.Failure;
                    }
                }
            }

            putInformation(info);
            return Try.Success;
        }
        private bool isCorrectRemoval(TileInformation info)
        {
            var pos = info.position;
            var id = info.entityId;


            for (int i = pos.left; i < pos.right; i++)
            {
                for (int j = pos.top; j < pos.bottom; j++)
                {
                    if (map[i, j] == null) return false;
                    if (map[i, j].entityId != id) return false;
                }
            }

            var actualPositionInMap = map[pos.x, pos.y].position;

            if (pos != actualPositionInMap)
            {
                throw new Exception();//testing if good !=
                return false;
            }

            return true;
        }

        public Try tryRemoveInformation(TileInformation info)
        {
            System.Console.WriteLine("WORLDMAP try remove information id " + info.entityId + " x " + info.position.x + " y " + info.position.y);
            if (!isCorrectRemoval(info)) return Try.Failure;

            removeInformation(info);
            return Try.Success;
        }

        private void putInformation(TileInformation info)
        {
            var pos = info.position;

            for (int i = pos.left; i < pos.right; i++)
            {
                for (int j = pos.top; j < pos.bottom; j++)
                {
                    map[i, j] = info;
                }
            }
            System.Console.WriteLine("WORDLMAP puted information id " + info.entityId + " x " + info.position.x + " y " + info.position.y);
        }

        private void removeInformation(TileInformation info)
        {
            var pos = info.position;

            for (int i = pos.left; i < pos.right; i++)
            {
                for (int j = pos.top; j < pos.bottom; j++)
                {
                    map[i, j] = null;
                }
            }
            System.Console.WriteLine("WORDLMAP removed information " + info.entityId + " x " + info.position.x + " y " + info.position.y);
        }


    }
}
