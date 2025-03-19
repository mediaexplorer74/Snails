
// Type: TwoBrainsGames.Snails.StageObjects.StageObjectFactory
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.Snails.StageObjects
{
  public class StageObjectFactory
  {
    private static StageObjectFactory.CreateObjectDelegate[] _constructors;

    public static void Initialize()
    {
      StageObjectFactory._constructors = new StageObjectFactory.CreateObjectDelegate[52];
      StageObjectFactory._constructors[1] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSnail);
      StageObjectFactory._constructors[2] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateStageEntrance);
      StageObjectFactory._constructors[3] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateStageExit);
      StageObjectFactory._constructors[4] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateApple);
      StageObjectFactory._constructors[5] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateVitamin);
      StageObjectFactory._constructors[6] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSnailCounter);
      StageObjectFactory._constructors[7] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateDynamite);
      StageObjectFactory._constructors[8] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateBox);
      StageObjectFactory._constructors[9] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateCopper);
      StageObjectFactory._constructors[12] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateExplosion);
      StageObjectFactory._constructors[13] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateTrampoline);
      StageObjectFactory._constructors[14] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSpikes);
      StageObjectFactory._constructors[15] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateTriggerSwitch);
      StageObjectFactory._constructors[16] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateTeleportEntrance);
      StageObjectFactory._constructors[17] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateTeleportExit);
      StageObjectFactory._constructors[18] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreatePickableObject);
      StageObjectFactory._constructors[19] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateProp);
      StageObjectFactory._constructors[20] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateFire);
      StageObjectFactory._constructors[21] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSalt);
      StageObjectFactory._constructors[22] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateDynamiteBox);
      StageObjectFactory._constructors[23] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateDynamiteBoxTriggered);
      StageObjectFactory._constructors[24] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSnailSacrifice);
      StageObjectFactory._constructors[25] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSnailKing);
      StageObjectFactory._constructors[26] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreatePopUpBox);
      StageObjectFactory._constructors[27] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLamp);
      StageObjectFactory._constructors[28] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateFlameLight);
      StageObjectFactory._constructors[29] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateWater);
      StageObjectFactory._constructors[30] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateCrystal);
      StageObjectFactory._constructors[31] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateStageProp);
      StageObjectFactory._constructors[32] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateInformationSign);
      StageObjectFactory._constructors[33] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSnailShell);
      StageObjectFactory._constructors[34] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateFadeInOutBox);
      StageObjectFactory._constructors[35] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateC4);
      StageObjectFactory._constructors[36] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLaserBeam);
      StageObjectFactory._constructors[37] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLaserBeamMirror);
      StageObjectFactory._constructors[38] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateControllableLaserCannon);
      StageObjectFactory._constructors[39] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLaserBeamSwitch);
      StageObjectFactory._constructors[40] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateAcid);
      StageObjectFactory._constructors[41] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLava);
      StageObjectFactory._constructors[42] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateWaterBubble);
      StageObjectFactory._constructors[43] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLiquidPump);
      StageObjectFactory._constructors[44] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLiquidPipe);
      StageObjectFactory._constructors[45] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateLiquidTap);
      StageObjectFactory._constructors[46] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateEvilSnail);
      StageObjectFactory._constructors[47] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateFixedLaserCannon);
      StageObjectFactory._constructors[48] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateDirectionalBox);
      StageObjectFactory._constructors[49] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateTutorialSign);
      StageObjectFactory._constructors[50] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateDynamiteBoxCounted);
      StageObjectFactory._constructors[51] = new StageObjectFactory.CreateObjectDelegate(StageObjectFactory.CreateSlime);
    }

    public static StageObject Create(StageObjectType type)
    {
      return StageObjectFactory._constructors[(int) type]();
    }

    private static Snail CreateSnail() => new Snail();

    private static StageExit CreateStageExit() => new StageExit();

    private static StageEntrance CreateStageEntrance() => new StageEntrance();

    private static SnailCounter CreateSnailCounter() => new SnailCounter();

    private static Dynamite CreateDynamite() => new Dynamite();

    private static Vitamin CreateVitamin() => new Vitamin();

    private static Box CreateBox() => new Box();

    private static Copper CreateCopper() => new Copper();

    private static CollisionTester CreateCollisionTeste() => new CollisionTester();

    private static Explosion CreateExplosion() => new Explosion();

    private static Apple CreateApple() => new Apple();

    private static Trampoline CreateTrampoline() => new Trampoline();

    private static Spikes CreateSpikes() => new Spikes();

    private static SnailTriggerSwitch CreateTriggerSwitch() => new SnailTriggerSwitch();

    private static TeleportEntrance CreateTeleportEntrance() => new TeleportEntrance();

    private static TeleportExit CreateTeleportExit() => new TeleportExit();

    private static PickableObject CreatePickableObject() => new PickableObject();

    private static Prop CreateProp() => new Prop();

    private static Fire CreateFire() => new Fire();

    private static Salt CreateSalt() => new Salt();

    private static DynamiteBox CreateDynamiteBox() => new DynamiteBox();

    private static DynamiteBoxTriggered CreateDynamiteBoxTriggered() => new DynamiteBoxTriggered();

    private static SnailSacrificeSwitch CreateSnailSacrifice() => new SnailSacrificeSwitch();

    private static SnailKing CreateSnailKing() => new SnailKing();

    private static PopUpBox CreatePopUpBox() => new PopUpBox();

    private static Lamp CreateLamp() => new Lamp();

    private static FlameLight CreateFlameLight() => new FlameLight();

    private static Water CreateWater() => new Water();

    private static Crystal CreateCrystal() => new Crystal();

    private static StageProp CreateStageProp() => new StageProp();

    private static InformationSign CreateInformationSign() => new InformationSign();

    private static SnailShell CreateSnailShell() => new SnailShell();

    private static FadeInOutBox CreateFadeInOutBox() => new FadeInOutBox();

    private static C4 CreateC4() => new C4();

    private static LaserBeam CreateLaserBeam() => new LaserBeam();

    private static LaserBeamMirror CreateLaserBeamMirror() => new LaserBeamMirror();

    private static ControllableLaserCannon CreateControllableLaserCannon()
    {
      return new ControllableLaserCannon();
    }

    private static LaserBeamSwitch CreateLaserBeamSwitch() => new LaserBeamSwitch();

    private static Acid CreateAcid() => new Acid();

    private static Lava CreateLava() => new Lava();

    private static WaterBubble CreateWaterBubble() => new WaterBubble();

    private static LiquidPipe CreateLiquidPipe() => new LiquidPipe();

    private static LiquidPump CreateLiquidPump() => new LiquidPump();

    private static LiquidTap CreateLiquidTap() => new LiquidTap();

    private static EvilSnail CreateEvilSnail() => new EvilSnail();

    private static FixedLaserCannon CreateFixedLaserCannon() => new FixedLaserCannon();

    private static DirectionalBox CreateDirectionalBox() => new DirectionalBox();

    private static TutorialSign CreateTutorialSign() => new TutorialSign();

    private static DynamiteBoxCounted CreateDynamiteBoxCounted() => new DynamiteBoxCounted();

    private static Slime CreateSlime() => new Slime();

    private delegate StageObject CreateObjectDelegate();
  }
}
