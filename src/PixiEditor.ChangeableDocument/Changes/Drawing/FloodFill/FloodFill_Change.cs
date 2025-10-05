﻿using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.Numerics;
using Drawie.Backend.Core.Vector;
using Drawie.Numerics;
using PixiEditor.ChangeableDocument.Changeables.Graph.Nodes;

namespace PixiEditor.ChangeableDocument.Changes.Drawing.FloodFill;

internal class FloodFill_Change : Change
{
    private readonly Guid memberGuid;
    private readonly VecI pos;
    private readonly Color color;
    private readonly bool referenceAll;
    private readonly bool drawOnMask;
    private CommittedChunkStorage? chunkStorage = null;
    private int frame;
    private float tolerance;

    [GenerateMakeChangeAction]
    public FloodFill_Change(Guid memberGuid, VecI pos, Color color, bool referenceAll, float tolerance, bool drawOnMask, int frame)
    {
        this.memberGuid = memberGuid;
        this.pos = pos;
        this.color = color;
        this.referenceAll = referenceAll;
        this.drawOnMask = drawOnMask;
        this.frame = frame;
        this.tolerance = tolerance;
    }

    public override bool InitializeAndValidate(Document target)
    {
        if (pos.X < 0 || pos.Y < 0 || pos.X >= target.Size.X || pos.Y >= target.Size.Y)
            return false;

        return DrawingChangeHelper.IsValidForDrawing(target, memberGuid, drawOnMask);
    }

    public override OneOf<None, IChangeInfo, List<IChangeInfo>> Apply(Document target, bool firstApply, out bool ignoreInUndo)
    {
        var image = DrawingChangeHelper.GetTargetImageOrThrow(target, memberGuid, drawOnMask, frame);

        VectorPath? selection = target.Selection.SelectionPath.IsEmpty ? null : target.Selection.SelectionPath;
        HashSet<Guid> membersToReference = new();
        if (referenceAll)
            target.ForEveryReadonlyMember(member => membersToReference.Add(member.Id));
        else
            membersToReference.Add(memberGuid);
        bool lockTransparency = target.FindMember(memberGuid) is ImageLayerNode { LockTransparency: true };
        var floodFilledChunks = FloodFillHelper.FloodFill(membersToReference, target, selection, pos, color, tolerance, frame, lockTransparency);
        if (floodFilledChunks.Count == 0)
        {
            ignoreInUndo = true;
            return new None();
        }

        foreach (var (chunkPos, chunk) in floodFilledChunks)
        {
            image.EnqueueDrawTexture(chunkPos * ChunkyImage.FullChunkSize, chunk.Surface, null, false);
        }
        var affArea = image.FindAffectedArea();
        chunkStorage = new CommittedChunkStorage(image, affArea.Chunks);
        image.CommitChanges();
        foreach (var chunk in floodFilledChunks.Values)
            chunk.Dispose();

        ignoreInUndo = false;
        return DrawingChangeHelper.CreateAreaChangeInfo(memberGuid, affArea, drawOnMask);
    }

    public override OneOf<None, IChangeInfo, List<IChangeInfo>> Revert(Document target)
    {
        var affArea = DrawingChangeHelper.ApplyStoredChunksDisposeAndSetToNull(target, memberGuid, drawOnMask, frame, ref chunkStorage);
        return DrawingChangeHelper.CreateAreaChangeInfo(memberGuid, affArea, drawOnMask);
    }

    public override void Dispose()
    {
        chunkStorage?.Dispose();
    }
}
