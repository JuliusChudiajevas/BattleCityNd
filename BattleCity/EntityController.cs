using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace BattleCity
{
    class EntityController
    {
        List<MyEntity> entities = new List<MyEntity>();
        WorldMap _map;
        KeyboardController _keyboardController;
        Control.ControlCollection _pictureBoxBucket;

        private bool buildPhase = true;

        public EntityController(WorldMap map, KeyboardController keyboardController, Control.ControlCollection pictureBoxBucket)
        {
            _map = map;
            _keyboardController = keyboardController;
            _pictureBoxBucket = pictureBoxBucket;
        }

        public void startGame()
        {
            var buildSquare = entities.FirstOrDefault(e => e is BuildSquare);
            purgeEntity(buildSquare.id);

            var playerSquare = new PlayerSquare(generateId());
            playerSquare.setPosition(playerSquare.spawnPosition);
            registerEntity(playerSquare);
            _map.tryPutInformation(new TileInformation(playerSquare.id, playerSquare.position));

            buildPhase = false;
        }

        public void initialize()
        {
            spawnEnemySpawns();
            spawnBuildSquare();
            fillMapBorders();
        }

        private void spawnEnemySpawns()
        {
            var enemySpawn = insertEnemySpawn();
            enemySpawn.setPosition(new Rect(100, 100));
            var response = _map.tryPutInformation(new TileInformation(enemySpawn.id, enemySpawn.position));
            if (response == Try.Failure)
            {
                System.Console.WriteLine("spawn enemySpawn fail x " + enemySpawn.position.x + " y " + enemySpawn.position.y);
            }
        }

        private void spawnBuildSquare()
        {
            var buildSquare = createBuildSquare();
            buildSquare.setPosition(buildSquare.spawnPosition);
            var response = _map.tryPutInformation(new TileInformation(buildSquare.id, buildSquare.position));
            if (response == Try.Failure)
            {
                System.Console.WriteLine("spawn build square fail x " + buildSquare.position.x + " y " + buildSquare.position.y);
            }
        }

        private void fillMapBorders()
        {
            var mapWidth = _map.getWidth;
            var mapHeight = _map.getHeight;
            var dummyBorder = new MapBorder(-999);
            var borderDimensions = dummyBorder.position;

            //top line
            for (int x = 0; x + borderDimensions.width < mapWidth; x += borderDimensions.width)
            {
                var y = 0;
                var border = insertMapBorder();
                border.setPosition(new Rect(x, y, border.position.width, border.position.height));
                var response = _map.tryPutInformation(new TileInformation(border.id, border.position));
                if (response == Try.Failure)
                {
                    System.Console.WriteLine("border fail x " + border.position.x + " y " + border.position.y);
                }
            }

            //top line
            for (int x = 0; x + borderDimensions.width < mapWidth; x += borderDimensions.width)
            {
                var y = mapHeight - borderDimensions.height;
                var border = insertMapBorder();
                border.setPosition(new Rect(x, y, border.position.width, border.position.height));
                var response = _map.tryPutInformation(new TileInformation(border.id, border.position));
                if (response == Try.Failure)
                {
                    System.Console.WriteLine("border fail x " + border.position.x + " y " + border.position.y);
                }
            }

            //left line
            for (int y = 0; y + borderDimensions.height < mapHeight; y += borderDimensions.height)
            {
                var x = 0;
                var border = insertMapBorder();
                border.setPosition(new Rect(x, y, border.position.width, border.position.height));
                var response = _map.tryPutInformation(new TileInformation(border.id, border.position));
                if (response == Try.Failure)
                {
                    System.Console.WriteLine("border fail x " + border.position.x + " y " + border.position.y);
                }
            }

            //right line
            for (int y = 0; y + borderDimensions.height <= mapHeight; y += borderDimensions.height)
            {
                var x = mapWidth - borderDimensions.width;
                var border = insertMapBorder();
                border.setPosition(new Rect(x, y, border.position.width, border.position.height));
                var response = _map.tryPutInformation(new TileInformation(border.id, border.position));
                if (response == Try.Failure)
                {
                    System.Console.WriteLine("border fail x " + border.position.x + " y " + border.position.y);
                }
            }
        }

        private void updateKeyboard()
        {
            var keyboardEntities = entities.Where(e => (e as IKeyboardSupport) != null).Cast<IKeyboardSupport>().ToList();
            keyboardEntities.ForEach(e => _keyboardController.relayKeys(e));
        }

        private void consumeCreateWallRequests(List<CreateWallRequest> requests)
        {
            foreach (var request in requests)
            {
                var wall = new Wall(generateId());
                wall.setPosition(request.position);

                System.Console.WriteLine("wall put attemp id " + wall.id + " x " + wall.position.x + " y " + wall.position.y);
                var response = _map.tryPutInformation(new TileInformation(wall.id, wall.position));
                if (response == Try.Failure)
                {
                    System.Console.WriteLine("wall put failure id " + wall.id + " x " + wall.position.x + " y " + wall.position.y);
                    // deleteEntity(wall.id);
                    return;
                }

                registerEntity(wall);

                System.Console.WriteLine("wall put success id " + wall.id + " x " + wall.position.x + " y " + wall.position.y);
            }
        }

        private void updateWallCreators()
        {
            var wallCreators = entities.Where(e => (e as IWallCreator) != null).Cast<IWallCreator>().ToList();
            wallCreators.ForEach(w => consumeCreateWallRequests(w.createWallRequests));
        }

        private void consumeRemoveWallRequests(List<RemoveWallRequest> requests)
        {
            foreach (var request in requests)
            {
                var tryExisitngInfo = _map.tryGetInfoFromPosition(request.position);
                if (tryExisitngInfo.result == Try.Failure)
                {
                    System.Console.WriteLine("removing wall fail no existing info");
                    return;
                }

                var info = tryExisitngInfo.info;
                var response = _map.tryRemoveInformation(tryExisitngInfo.info);
                if (response == Try.Failure) System.Console.WriteLine("wall remove failure id " + info.entityId
                                                                            + " x " + info.position.x + " y " + info.position.y);

                if (response == Try.Success)
                {
                    System.Console.WriteLine("removing a wall");
                    purgeEntity(info.entityId);
                }
            }
        }

        private void updateWallRemovers()
        {
            var wallCreators = entities.Where(e => e is IWallRemover).Cast<IWallRemover>().ToList();
            wallCreators.ForEach(w => consumeRemoveWallRequests(w.removeWallRequests));
        }

        private void consumeCreateBulletRequests(List<CreateBulletRequest> requests)
        {
            foreach (var request in requests)
            {
                var bullet = new Bullet(generateId());
                bullet.setPosition(request.position);
                bullet.direction = request.direction;
                var putResponse = _map.tryPutInformation(new TileInformation(bullet.id, bullet.position));
                if (putResponse == Try.Failure)
                {
                    // purgeEntity(bullet.id);
                    System.Console.WriteLine("bullet put failure id " + bullet.id + " x " + bullet.position.x + " y " + bullet.position.y);
                    return;
                }

                registerEntity(bullet);
                System.Console.WriteLine("bullet put success id " + bullet.id + " x " + bullet.position.x + " y " + bullet.position.y);
            }
        }

        private void updateBulletCreators()
        {
            var bulletCreators = entities.Where(e => e is IBulletCreator).Cast<IBulletCreator>().ToList();
            bulletCreators.ForEach(w => consumeCreateBulletRequests(w.createBulletRequests));
        }

        private void moveBuildSquare()
        {
            var moveableEntities = entities.Where(e => e is BuildSquare).Cast<BuildSquare>().ToList();
            foreach (var entity in moveableEntities)
            {
                var oldPosition = entity.getPosition();
                var newPosition = entity.desiredNewPosition;

                if (oldPosition.x == newPosition.x && oldPosition.y == newPosition.y) continue;

                // var moveResponse = _map.tryMoveInformation(new MoveRequest(oldPosition, newPosition));
                // if (moveResponse.result == Try.Failure)
                // {
                //     System.Console.WriteLine("move failure id " + ((MyEntity)entity).id + " x " + ((MyEntity)entity).position.x + " y " + ((MyEntity)entity).position.y);
                //     return;
                // }
                entity.setPosition(newPosition);
            }
        }


        private void moveEntities()
        {
            var moveableEntities = entities.Where(e => e is IMovable).Cast<IMovable>().ToList();
            foreach (var entity in moveableEntities)
            {
                var oldPosition = entity.getPosition();
                var newPosition = entity.desiredNewPosition;

                if (oldPosition.x == newPosition.x && oldPosition.y == newPosition.y) continue;

                System.Console.WriteLine("EntityController moving entity id " + ((MyEntity)entity).id + "type " + ((MyEntity)entity).type + " old x "
                        + oldPosition.x + " old y " + oldPosition.y + " new x " + newPosition.x + " new y " + newPosition.y);

                var moveResponse = _map.tryMoveInformation(new MoveRequest(oldPosition, newPosition));
                if (moveResponse.result == Try.Failure)
                {
                    System.Console.WriteLine("move failure id " + ((MyEntity)entity).id + " x " + ((MyEntity)entity).position.x + " y " + ((MyEntity)entity).position.y);
                    continue;
                }
                entity.setPosition(newPosition);
            }
        }

        public void update()
        {
            updateKeyboard();
            updateWallCreators();
            updateWallRemovers();
            updateBulletCreators();
            if (buildPhase) moveBuildSquare(); else moveEntities();
        }

        private BuildSquare createBuildSquare()
        {
            var id = generateId();
            var buildSquare = new BuildSquare(id);

            entities.Add(buildSquare);
            _pictureBoxBucket.Add(buildSquare.pictureBox);
            buildSquare.setPosition(buildSquare.spawnPosition);
            _keyboardController.registerKeys(buildSquare);
            return buildSquare;
        }
        private void registerEntity(MyEntity entity)
        {
            entities.Add(entity);
            _pictureBoxBucket.Add(entity.pictureBox);
        }

        private Wall insertWall()
        {
            var id = generateId();
            var entity = new Wall(id);

            entities.Add(entity);
            _pictureBoxBucket.Add(entity.pictureBox);

            return entity;
        }

        private BuildSquare insertBuildSquare()
        {
            var id = generateId();
            var entity = new BuildSquare(id);

            entities.Add(entity);
            _pictureBoxBucket.Add(entity.pictureBox);

            return entity;
        }

        private Bullet inserBullet()
        {
            var id = generateId();
            var entity = new Bullet(id);

            entities.Add(entity);
            _pictureBoxBucket.Add(entity.pictureBox);

            return entity;
        }

        private PlayerSquare insertPlayerSquare()
        {
            var id = generateId();
            var entity = new PlayerSquare(id);

            entities.Add(entity);
            _pictureBoxBucket.Add(entity.pictureBox);

            return entity;
        }

        private EnemySpawn insertEnemySpawn()
        {
            var id = generateId();
            var enemySpawn = new EnemySpawn(id);

            entities.Add(enemySpawn);
            _pictureBoxBucket.Add(enemySpawn.pictureBox);

            return enemySpawn;
        }

        private MapBorder insertMapBorder()
        {
            var id = generateId();
            var border = new MapBorder(id);

            entities.Add(border);
            _pictureBoxBucket.Add(border.pictureBox);

            return border;
        }

        // public Try createEntity(EntityType type)
        // {
        //     switch (type)
        //     {
        //         case EntityType.BuildSquare:
        //             createBuildSquare();
        //             return Try.Success;

        //         case EntityType.Wall:
        //             insertWall();
        //             return Try.Success;

        //         default: return Try.Failure;
        //     }
        // }
        public void purgeEntity(int id)
        {
            var entity = entities.FirstOrDefault(e => e.id == id);
            System.Console.WriteLine("deleting entity id" + entity.id + " type " + entity.type + " x " + entity.position.x + " y " + entity.position.y);
            entities.Remove(entity);
            _pictureBoxBucket.Remove(entity.pictureBox);
            _map.tryRemoveInformation(new TileInformation(entity.id, entity.position));
        }

        private int lastId = 0;
        private int generateId()
        {
            lastId++;
            return lastId;
        }
    }
}
