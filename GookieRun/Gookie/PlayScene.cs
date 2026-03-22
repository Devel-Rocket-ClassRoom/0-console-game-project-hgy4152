using Framework.Engine;
using System;
using System.Collections.Generic;
using System.Text;

class PlayScene : Scene
{
    public event GameAction PlayAgainRequested;


    public event Action<Skill> OnSkill; 
    public event Action<Skill> OffSkill;
    public event Action Dead;

    private Map map;
    private Gookie gookie;

    private bool isCrash = false;
    private bool isSkill = false;
    private bool isDead = false;
    private float immuneTime = 0f;
    private float playTime = 0f;

    private int score = 0;
    private int health = 60;


    public override void Update(float deltaTime)
    {
        UpdateGameObjects(deltaTime);

        if(isDead)
        {
            Dead();
            return;
        }
        // 스킬 사용
        // 이벤트로 받아서 map, gookie에 발송 해주는 식으로 바꾸기
        // map 은 스피드 변환
        // gookie는 위치 변환 및 히트박스 확대
        isSkill = gookie.isSkill;
        if (isSkill)
        {
            OnSkill(gookie.skill);
        }
        else
        {
            OffSkill(gookie.skill);
        }

        // 장애물 충돌 후 무적
        if (isCrash)
        {
            immuneTime += deltaTime;

            if(immuneTime > 2)
            {
                isCrash = false;
                immuneTime = 0f;
            }
           
        }

        // 먹은거나 부딫힌거 검사 및 제거
        map.mapObj.RemoveAll(pos =>
        {

            if (gookie.isBound(pos.X, pos.Y, pos.Name, pos.Type))
            {

                pos.destroy = isSkill;

                return Judge(pos.Type, pos.Name);
            }


            return false;
        });

        playTime += deltaTime;
        // 체력 자동 감소
        if (playTime > 1 && !isSkill)
        {
            health -= 1;
            playTime = 0;
        }

    }
    public override void Draw(ScreenBuffer buffer)
    {
        DrawGameObjects(buffer);

        buffer.WriteText(1, 0, $"HP:", ConsoleColor.Green);
        buffer.DrawHLine(5, 0, health, '█', ConsoleColor.Green);

        buffer.WriteText(7, 16, $"Score: {score}", ConsoleColor.Cyan);

    }

    public override void Load()
    {
        map = new Map(this);
        AddGameObject(map);

        OnSkill += map.mapReactionOn;
        OffSkill += map.mapReactionOff;
        Dead += map.Dead;

        // 쿠키, 스킬 셋팅 등록
        gookie = new Gookie(this);

        AddGameObject(gookie);
        AddGameObject(gookie.skill);

        OnSkill += gookie.OnDash;
        OffSkill += gookie.OffDash;
        Dead += gookie.Dead;
        Dead += gookie.skill.Dead;

        Console.OutputEncoding = Encoding.UTF8;

    }

    public override void Unload()
    {
        ClearGameObjects();

    }


    bool Judge(string type , string name)
    {
        if(type == "Jelly")
        {
            if(name == "Normal")
            {
                score += 10;
            }
            else
            {
                score += 50;
            }

            return true;
            
        }
        else if(type == "Potion")
        {
            health += 5;

            return true;
        }

        else if(type == "Obstarcle" && !isCrash)
        {

            isCrash = true;

            // 파괴 점수
            if(isSkill)
            {
                score += 100;
            }
            else
            {
                health -= 5;
            }
            
        }

        else if(type == "Dead")
        {
            isDead = !gookie.isJump;
        }
        return false;
    }


}