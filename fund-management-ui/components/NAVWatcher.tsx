"use client";

import { useState } from "react";
import { useNAVUpdates, ConnectionStatus } from "@/hooks/useNAVUpdates";

const statusConfig: Record<ConnectionStatus, { dot: string; label: string; pulse: boolean }> = {
  disconnected: { dot: "bg-red-500", label: "Disconnected", pulse: false },
  connecting: { dot: "bg-yellow-400", label: "Connecting…", pulse: true },
  connected: { dot: "bg-green-500", label: "Connected", pulse: false },
};

export default function NAVWatcher() {
  const [inputId, setInputId] = useState("1");
  const [watchedId, setWatchedId] = useState<number | null>(null);
  const { updates, status } = useNAVUpdates(watchedId);

  const { dot, label, pulse } = statusConfig[status];

  const handleSubscribe = () => {
    const id = parseInt(inputId, 10);
    if (!isNaN(id) && id > 0) setWatchedId(id);
  };

  const handleUnsubscribe = () => setWatchedId(null);

  return (
    <div className="w-full max-w-md space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-bold tracking-tight">Live NAV Updates</h1>
        <p className="text-sm text-zinc-500 mt-1">
          Real-time fund NAV via SignalR WebSocket
        </p>
      </div>

      {/* Connection status */}
      <div className="flex items-center gap-2">
        <span className={`h-2.5 w-2.5 rounded-full ${dot} ${pulse ? "animate-pulse" : ""}`} />
        <span className="text-sm text-zinc-500">{label}</span>
        {watchedId !== null && status === "connected" && (
          <span className="text-sm text-zinc-400">— fund #{watchedId}</span>
        )}
      </div>

      {/* Controls */}
      <div className="flex gap-2">
        <input
          type="number"
          min="1"
          value={inputId}
          onChange={(e) => setInputId(e.target.value)}
          onKeyDown={(e) => e.key === "Enter" && handleSubscribe()}
          placeholder="Fund ID"
          className="flex-1 rounded-lg border border-zinc-200 dark:border-zinc-700 bg-white dark:bg-zinc-900 px-4 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-zinc-400"
        />
        {watchedId === null ? (
          <button
            onClick={handleSubscribe}
            className="rounded-lg bg-zinc-900 dark:bg-zinc-50 px-5 py-2 text-sm font-medium text-white dark:text-zinc-900 hover:opacity-80 transition-opacity"
          >
            Subscribe
          </button>
        ) : (
          <button
            onClick={handleUnsubscribe}
            className="rounded-lg border border-zinc-200 dark:border-zinc-700 px-5 py-2 text-sm font-medium text-zinc-600 dark:text-zinc-400 hover:bg-zinc-100 dark:hover:bg-zinc-800 transition-colors"
          >
            Unsubscribe
          </button>
        )}
      </div>

      {/* Feed */}
      <div className="rounded-xl border border-zinc-200 dark:border-zinc-800 overflow-hidden">
        <div className="flex items-center justify-between bg-zinc-50 dark:bg-zinc-900 px-4 py-2.5 border-b border-zinc-200 dark:border-zinc-800">
          <span className="text-xs font-semibold uppercase tracking-wider text-zinc-400">
            Update Feed
          </span>
          {updates.length > 0 && (
            <span className="text-xs text-zinc-400">
              {updates.length} update{updates.length !== 1 ? "s" : ""}
            </span>
          )}
        </div>

        <div className="max-h-72 overflow-y-auto divide-y divide-zinc-100 dark:divide-zinc-800">
          {updates.length === 0 ? (
            <p className="px-4 py-8 text-center text-sm text-zinc-400">
              {watchedId === null
                ? "Enter a fund ID and click Subscribe."
                : "Waiting for NAV updates…"}
            </p>
          ) : (
            updates.map((u, i) => (
              <div
                key={`${u.fundId}-${u.updatedAt}-${i}`}
                className={`flex items-center justify-between px-4 py-3 transition-colors ${
                  i === 0
                    ? "bg-green-50 dark:bg-green-950/20"
                    : "bg-white dark:bg-zinc-950"
                }`}
              >
                <div className="flex items-center gap-2">
                  <span className="text-xs font-mono text-zinc-400">
                    Fund #{u.fundId}
                  </span>
                  {i === 0 && (
                    <span className="text-[10px] font-semibold px-1.5 py-0.5 rounded bg-green-100 dark:bg-green-900/40 text-green-700 dark:text-green-400">
                      LIVE
                    </span>
                  )}
                </div>
                <div className="text-right">
                  <p className="text-sm font-semibold font-mono">
                    {u.nav.toFixed(4)}
                  </p>
                  <p className="text-xs text-zinc-400">
                    {new Date(u.updatedAt).toLocaleTimeString()}
                  </p>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </div>
  );
}
