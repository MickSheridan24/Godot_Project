
public enum eCollisionLayers
{
    ENTITY = 0,
    HOSTILE = 1,
    FRIENDLY = 2,
    INERT = 3,
    INTANGIBLE = 4,


    LEVEL1 = 5,
    LEVEL2 = 6,
    LEVEL3 = 7,
    LEVEL4 = 8,
    LEVEL5 = 9
}


public enum eSpell
{
    FIREBALL,
    LIGHTNING,
    EARTH_WALL,
    SPEED,
    CREATE_GRASS,
    CREATE_FARM
}
public enum eDamageType
{
    FIRE,
    ELECTRIC,
    PHYSICAL
}


public enum eProjectileType
{
    FIREBALL,
    LIGHTNING
}


public enum eTileType
{
    DIRT = 0,
    GRASS = 1,
    EARTH_WALL = 2,
}

public enum eStat
{
    SPEED,
    HEALTH,
    DAMAGE,
    RANGE
}

public enum eStatusEffect
{
    JOLTED,
    BURNING,
    FREEZING,
    INTANGIBLE
}

public enum eTeam
{
    FRIENDLY,
    HOSTILE,
    NEUTRAL
}